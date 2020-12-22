using Akka.Actor;
using Akka.DI.Core;
using Akka.Monitoring;
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
    /// Parent of multiple ModuleActors
    /// </summary>
    public class DataOwnerActor : AbstractPersistedWorkflowActor<IModuleCRUDMessage, Dictionary<string, IModuleCRUDMessage>>
    {
        public DataOwnerActor()
        {
            Command<IModuleCRUDMessage>(message =>
            {
                Context.IncrementMessagesReceived();
                PersistWorkflowData(message);
                ModuleCRUDMessageHandler(message);
            });

            Command<LookupModuleMessage>(message =>
            {
                Context.IncrementMessagesReceived();
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

            Command<ListAllModulesMessage>(message =>
            {
                Context.IncrementMessagesReceived();
                ListAllModulesMessageHandler(message);
            });

            #region Ignored Messages

            Command<CreateWorkflowFailedMessage>(message => {
                //ignore
            });
            Command<CreateWorkflowSucceededMessage>(message => {
                //ignore
            });

            #endregion
        }

        #region Handlers

        private void LookupModuleMessageHandler(LookupModuleMessage message)
        {
            IActorRef actorRef = LookupModule(message.ModuleKey, message.ModuleVersion);
            if (actorRef == ActorRefs.Nobody)
            {
                Sender.Tell(new LookupModuleFailedMessage(message));
            }
            else
            {
                Sender.Tell(new LookupModuleSucceededMessage(message, actorRef));
            }
        }

        private void LookupWorkflowMessageHandler(LookupWorkflowMessage message)
        {
            IActorRef actorRef = LookupModule(message.ModuleKey, message.ModuleVersion);
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
            IActorRef actorRef = LookupModule(message.ModuleKey, message.ModuleVersion);
            if (actorRef == ActorRefs.Nobody)
            {
                Sender.Tell(new LookupSagaFailedMessage(message));
            }
            else
            {
                actorRef.Forward(message);
            }
        }

        private void ListAllModulesMessageHandler(ListAllModulesMessage message)
        {
            List<(KeyString, ReadOnlyCollection<SemanticVersionNumber>)> modules = new List<(KeyString, ReadOnlyCollection<SemanticVersionNumber>)>();

            foreach (var modulePair in _modules)
            {
                List<SemanticVersionNumber> versions = new List<SemanticVersionNumber>();
                foreach (var version in modulePair.Value.Keys)
                {
                    versions.Add(version);
                }
                modules.Add((modulePair.Key, versions.AsReadOnly()));
            }
            Sender.Tell(new ListAllModulesSucceededMessage(modules.AsReadOnly()));
        }

        protected override void RecoverPersistedWorkflowDataHandler(IModuleCRUDMessage data)
        {
            ModuleCRUDMessageHandler(data);
        }

        private void ModuleCRUDMessageHandler(IModuleCRUDMessage message)
        {
            switch (message.CRUDMessageType)
            {
                case Model.CRUDMessageType.Create:
                    CreateModuleMessage createModuleMessage = message as CreateModuleMessage;
                    string name = createModuleMessage.ModuleDefinition.Name;
                    SemanticVersionNumber version = createModuleMessage.ModuleDefinition.Version;
                    string instanceName = name + "." + version.ToString();
                    if (Context.Child(instanceName) == ActorRefs.Nobody)
                    {
                        try
                        {
                            var actorProps = Context.DI().Props<ModuleActor>();

                            var moduleRef = Context.ActorOf(actorProps, instanceName);

                            _modules.Add(name, new Dictionary<SemanticVersionNumber, IActorRef>() { { version, moduleRef } });

                            foreach (WorkflowDefinition workflowDefinition in createModuleMessage.ModuleDefinition.WorkflowDefinitions)
                            {
                                moduleRef.Tell(new CreateWorkflowMessage(workflowDefinition));
                            }
                            Sender.Tell(new CreateModuleSucceededMessage(createModuleMessage, moduleRef));
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = string.Format("{0} failed in {1}", ActorId, typeof(CreateModuleMessage).Name);
                            Logger.Error(ex, errorMessage);
                            Sender.Tell(new CreateModuleFailedMessage(createModuleMessage, ex, errorMessage));
                        }
                    }
                    else
                    {
                        string errorMessage = string.Format("{0} received a {1} for a already existing Module", ActorId, typeof(CreateModuleMessage).Name);
                        Logger.Debug(errorMessage);
                        Sender.Tell(new CreateModuleFailedMessage(createModuleMessage, errorMessage));
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

        private Dictionary<string, Dictionary<SemanticVersionNumber, IActorRef>> _modules = new Dictionary<string, Dictionary<SemanticVersionNumber, IActorRef>>();

        
        /// <summary>
        /// Looks up an active Module child. If version is not supplied the highest version is returned
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private IActorRef LookupModule(KeyString moduleKey, SemanticVersionNumber version = null)
        {
            if (_modules.TryGetValue(moduleKey, out Dictionary<SemanticVersionNumber, IActorRef> versions))
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
