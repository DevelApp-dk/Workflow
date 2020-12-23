using Akka.Actor;
using Akka.DI.Core;
using Akka.Monitoring;
using Default;
using DevelApp.Utility.Model;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.AbstractActors;
using DevelApp.Workflow.Core.Exceptions;
using DevelApp.Workflow.Core.Messages;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Interfaces;
using DevelApp.Workflow.Messages;
using DevelApp.Workflow.Model;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Command<IDataOwnerCRUDMessage>(message =>
            {
                Context.IncrementMessagesReceived();
                PersistWorkflowData(message);
                DataOwnerCRUDMessageHandler(message);
            });

            Command<LookupDataOwnerMessage>(message =>
            {
                Context.IncrementMessagesReceived();
                LookupDataOwnerMessageHandler(message);
            });

            Command<LookupModuleMessage>(message =>
            {
                LookupModuleMessageHandler(message);
            });

            Command<LookupWorkflowMessage>(message =>
            {
                LookupWorkflowMessageHandler(message);
            });

            Command<LookupSagaMessage>(message =>
            {
                LookupSagaMessageHandler(message);
            });

            Command<ListAllDataOwnersMessage>(message =>
            {
                Context.IncrementMessagesReceived();
                ListAllDataOwnersMessageHandler(message);
            });

            #region Ignored Messages

            Command<CreateModuleFailedMessage>(message => {
                //ignore
            });
            Command<CreateModuleSucceededMessage>(message => {
                //ignore
            });

            #endregion
        }

        #region Handlers

        private void LookupDataOwnerMessageHandler(LookupDataOwnerMessage message)
        {
            IActorRef actorRef = LookupDataOwner(message.DataOwnerKey, message.DataOwnerVersion);
            if(actorRef == ActorRefs.Nobody)
            {
                Sender.Tell(new LookupDataOwnerFailedMessage(message));
            }
            else
            {
                Sender.Tell(new LookupDataOwnerSucceededMessage(message, actorRef));
            }
        }

        private void LookupModuleMessageHandler(LookupModuleMessage message)
        {
            IActorRef actorRef = LookupDataOwner(message.DataOwnerKey, message.DataOwnerVersion);
            if (actorRef == ActorRefs.Nobody)
            {
                Sender.Tell(new LookupModuleFailedMessage(message));
            }
            else
            {
                actorRef.Forward(message);
            }
        }

        private void LookupWorkflowMessageHandler(LookupWorkflowMessage message)
        {
            IActorRef actorRef = LookupDataOwner(message.DataOwnerKey, message.DataOwnerVersion);
            if (actorRef == ActorRefs.Nobody)
            {
                Sender.Tell(new LookupWorkflowFailedMessage(message));
            }
            else
            {
                actorRef.Forward(message);
            }
        }

        private void LookupSagaMessageHandler(LookupSagaMessage message)
        {
            IActorRef actorRef = LookupDataOwner(message.DataOwnerKey, message.DataOwnerVersion);
            if (actorRef == ActorRefs.Nobody)
            {
                Sender.Tell(new LookupSagaFailedMessage(message));
            }
            else
            {
                actorRef.Forward(message);
            }
        }

        private void ListAllDataOwnersMessageHandler(ListAllDataOwnersMessage message)
        {
            List<(KeyString, ReadOnlyCollection<SemanticVersionNumber>)> dataOwners = new List<(KeyString, ReadOnlyCollection<SemanticVersionNumber>)>();

            foreach(var dataOwnerPair in _dataOwners)
            {
                List<SemanticVersionNumber> versions = new List<SemanticVersionNumber>();
                foreach(var version in dataOwnerPair.Value.Keys)
                {
                    versions.Add(version);
                }
                dataOwners.Add((dataOwnerPair.Key, versions.AsReadOnly()));
            }
            Sender.Tell(new ListAllDataOwnersSucceededMessage(dataOwners.AsReadOnly()));
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
                    SemanticVersionNumber version = createDataOwnerMessage.DataOwnerDefinition.Version;
                    string instanceName = $"{name}_{version}_1";//Instance number hardcoded
                    if (Context.Child(instanceName) == ActorRefs.Nobody)
                    {
                        try
                        {
                            var actorProps = Context.DI().Props<DataOwnerActor>();
                            //TODO Add router for the actor for different instances

                            var dataOwnerRef = Context.ActorOf(actorProps, instanceName);

                            _dataOwners.Add(name, new Dictionary<SemanticVersionNumber, IActorRef>() { {version, dataOwnerRef } });

                            foreach (ModuleDefinition moduleDefinition in createDataOwnerMessage.DataOwnerDefinition.ModuleDefinitions)
                            {
                                dataOwnerRef.Tell(new CreateModuleMessage(moduleDefinition));
                            }
                            Sender.Tell(new CreateDataOwnerSucceededMessage(createDataOwnerMessage, dataOwnerRef));
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = string.Format("{0} failed in {1}", ActorId, typeof(CreateDataOwnerMessage).Name);
                            Logger.Error(ex,errorMessage);
                            Sender.Tell(new CreateDataOwnerFailedMessage(createDataOwnerMessage, ex, errorMessage));
                        }
                    }
                    else
                    {
                        string errorMessage = string.Format("{0} received a {1} for a already existing DataOwner", ActorId, typeof(CreateDataOwnerMessage).Name);
                        Logger.Debug(errorMessage);
                        Sender.Tell(new CreateDataOwnerFailedMessage(createDataOwnerMessage, errorMessage));
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


        protected override void WorkflowMessageHandler(IWorkflowMessage message)
        {
            switch (message.MessageTypeName)
            {
                default:
                    Logger.Warning("{0} Did not handle received message [{1}] from [{2}]", ActorId, message.MessageTypeName, Sender.Path);
                    if (!Sender.IsNobody() && !message.IsReply)
                    {
                        Sender.Tell((message as WorkflowMessage).GetWorkflowUnhandledMessage("Message Type Not Implemented", Self.Path));
                    }
                    break;
            }
        }

        #endregion

        private Dictionary<string, Dictionary<SemanticVersionNumber, IActorRef>> _dataOwners = new Dictionary<string, Dictionary<SemanticVersionNumber, IActorRef>>();

        /// <summary>
        /// Looks up an acrive DataOwner child. If version is not supplied the highest version is returned
        /// </summary>
        /// <param name="dataOwnerKey"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private IActorRef LookupDataOwner(KeyString dataOwnerKey, SemanticVersionNumber version = null)
        {
            if (_dataOwners.TryGetValue(dataOwnerKey, out Dictionary<SemanticVersionNumber, IActorRef> versions))
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

        protected override void GroupFinishedMessageHandler(GroupFinishedMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
