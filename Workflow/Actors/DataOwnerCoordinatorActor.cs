using Akka.Actor;
using Akka.DI.Core;
using Akka.Monitoring;
using DevelApp.Workflow.Core.Exceptions;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Interfaces;
using DevelApp.Workflow.Messages;
using DevelApp.Workflow.Model;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevelApp.Workflow.Actors
{
    /// <summary>
    /// Singleton parent of potentially multiple DataOwnerActor
    /// </summary>
    public class DataOwnerCoordinatorActor : AbstractPersistedWorkflowActor<IDataOwnerCRUDMessage, Dictionary<string, IDataOwnerCRUDMessage>>
    {
        public DataOwnerCoordinatorActor()
        {
            Command<IDataOwnerCRUDMessage>(message => {
                Context.IncrementMessagesReceived();
                PersistWorkflowData(message);
                DataOwnerCRUDMessageHandler(message);
            });

            Command<LookupDataOwnerMessage>(message => {
                Context.IncrementMessagesReceived();
                LookupDataOwnerMessageHandler(message);
            });
        }

        #region Handlers

        private void LookupDataOwnerMessageHandler(LookupDataOwnerMessage message)
        {
            IActorRef actorRef = LookupDataOwner(message.DataOwnerKey, message.DataOwnerVersion);
            if(actorRef == ActorRefs.Nobody)
            {
                Sender.Tell(new LookupDataOwnerFailedMessage(message.DataOwnerKey));
            }
            else
            {
                Sender.Tell(new LookupDataOwnerSucceededMessage(message.DataOwnerKey, actorRef));
            }
        }

        protected override void RecoverPersistedWorkflowDataHandler(IDataOwnerCRUDMessage data)
        {
            DataOwnerCRUDMessageHandler(data);
        }

        private void DataOwnerCRUDMessageHandler(IDataOwnerCRUDMessage message)
        {
            switch (message.CRUDMessageType)
            {
                case Model.CRUDMessageType.Create:
                    CreateDataOwnerMessage createDataOwnerMessage = message as CreateDataOwnerMessage;
                    string name = createDataOwnerMessage.DataOwnerDefinition.Name;
                    int version = (int)createDataOwnerMessage.DataOwnerDefinition.Version;
                    string instanceName = BuildInstanceName(name, version);
                    if (Context.Child(instanceName) == ActorRefs.Nobody)
                    {
                        try
                        {
                            var actorProps = Context.DI().Props<DataOwnerActor>();
                            //TODO Add router for the actor for different instances

                            var myActorRef = Context.ActorOf(actorProps, instanceName);

                            _dataOwners.Add(createDataOwnerMessage.DataOwnerDefinition.Name, new Dictionary<int, IActorRef>() { {version, myActorRef } });

                            //TODO pass createDataOwnerMessage.DataOwnerDefinition.ModuleDefinitions to child

                            Sender.Tell(new CreateDataOwnerSucceededMessage(createDataOwnerMessage.DataOwnerKey, myActorRef));
                        }
                        catch (Exception ex)
                        {
                            Sender.Tell(new CreateDataOwnerFailedMessage(createDataOwnerMessage.DataOwnerKey, ex));
                        }
                    }
                    else
                    {
                        Logger.Debug("{0} received a {1} for a already existing DataOwner", ActorId, typeof(CreateDataOwnerMessage).Name);
                        Sender.Tell(new CreateDataOwnerFailedMessage(createDataOwnerMessage.DataOwnerKey));
                    }
                    break;
                case CRUDMessageType.Delete:
                    //Delete children with or without the data
                    throw new NotImplementedException();
                default:
                    //TODO delete
                    throw new WorkflowStartupException("CRUD Message Type Not Implemented");
            }
        }


        protected override void WorkflowMessageHandler(WorkflowMessage message)
        {
            switch (message.MessageTypeName)
            {
                default:
                    Logger.Warning("{0} Did not handle received message [{1}] from [{2}]", ActorId, message.MessageTypeName, message.OriginalSender);
                    Sender.Tell(new WorkflowUnhandledMessage(message, Self.Path));
                    break;
            }
        }

        #endregion

        private Dictionary<string, Dictionary<int, IActorRef>> _dataOwners = new Dictionary<string, Dictionary<int, IActorRef>>();

        protected override VersionNumber ActorVersion
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Looks up an acrive DataOwner child. If version is not supplied the highest version is returned
        /// </summary>
        /// <param name="dataOwnerKey"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private IActorRef LookupDataOwner(KeyString dataOwnerKey, VersionNumber version = null)
        {
            if (_dataOwners.TryGetValue(dataOwnerKey, out Dictionary<int, IActorRef> versions))
            {
                if (version == null && versions != null)
                {
                    return versions[versions.Max(v => v.Key)];
                }
                else if (versions != null && versions.TryGetValue(version, out IActorRef actorRef))
                {
                    return actorRef;
                }
            }
            return ActorRefs.Nobody;
        }

        /// <summary>
        /// Supervisory stategy for direct children with default handling
        /// </summary>
        /// <returns></returns>
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 10,
                withinTimeRange: TimeSpan.FromMinutes(1),
                localOnlyDecider: ex =>
                {
                    //Local
                    if (ex is ArithmeticException)
                    {
                        return Directive.Resume;
                    }

                    //Fallback to Default Stategy if not handled
                    return Akka.Actor.SupervisorStrategy.DefaultStrategy.Decider.Decide(ex);
                });
        }

        protected override void DoLastActionsAfterRecover()
        {
            Logger.Debug("{0} Finished restoring", ActorId);
        }
    }
}
