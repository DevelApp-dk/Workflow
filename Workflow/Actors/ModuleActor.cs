﻿using Akka.Actor;
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DevelApp.Workflow.Actors
{
    /// <summary>
    /// Holds the module informaion ande acts as coordinator for the children
    /// </summary>
    public class ModuleActor : AbstractPersistedWorkflowActor<IWorkflowCRUDMessage, Dictionary<string, IWorkflowCRUDMessage>>
    {
        public ModuleActor()
        {
            Command<IWorkflowCRUDMessage>(message =>
            {
                Context.IncrementMessagesReceived();
                PersistWorkflowData(message);
                WorkflowCRUDMessageHandler(message);
            });

            Command<LookupWorkflowMessage>(message =>
            {
                Context.IncrementMessagesReceived();
                LookupWorkflowMessageHandler(message);
            });

            Command<LookupSagaMessage>(message =>
            {
                LookupSagaMessageHandler(message);
            });

            Command<ListAllWorkflowsMessage>(message =>
            {
                Context.IncrementMessagesReceived();
                ListAllWorkflowsMessageHandler(message);
            });

            #region Ignored Messages

            Command<CreateSagaFailedMessage>(message => {
                //ignore
            });
            Command<CreateSagaSucceededMessage>(message => {
                //ignore
            });

            #endregion
        }

        #region Handlers

        private void LookupWorkflowMessageHandler(LookupWorkflowMessage message)
        {
            IActorRef actorRef = LookupWorkflow(message.WorkflowKey, message.WorkflowVersion);
            if (actorRef == ActorRefs.Nobody)
            {
                Sender.Tell(new LookupWorkflowFailedMessage(message));
            }
            else
            {
                Sender.Tell(new LookupWorkflowSucceededMessage(message, actorRef));
            }
        }

        private void LookupSagaMessageHandler(LookupSagaMessage message)
        {
            IActorRef actorRef = LookupWorkflow(message.ModuleKey, message.ModuleVersion);
            if (actorRef == ActorRefs.Nobody)
            {
                Sender.Tell(new LookupSagaFailedMessage(message));
            }
            else
            {
                actorRef.Forward(message);
            }
        }

        private void ListAllWorkflowsMessageHandler(ListAllWorkflowsMessage message)
        {
            List<(KeyString, ReadOnlyCollection<VersionNumber>)> workflows = new List<(KeyString, ReadOnlyCollection<VersionNumber>)>();

            foreach (var workflowPair in _workflows)
            {
                List<VersionNumber> versions = new List<VersionNumber>();
                foreach (var version in workflowPair.Value.Keys)
                {
                    versions.Add(version);
                }
                workflows.Add((workflowPair.Key, versions.AsReadOnly()));
            }
            Sender.Tell(new ListAllWorkflowsSucceededMessage(workflows.AsReadOnly()));
        }

        protected override void RecoverPersistedWorkflowDataHandler(IWorkflowCRUDMessage data)
        {
            WorkflowCRUDMessageHandler(data);
        }

        private void WorkflowCRUDMessageHandler(IWorkflowCRUDMessage message)
        {
            switch (message.CRUDMessageType)
            {
                case Model.CRUDMessageType.Create:
                    CreateWorkflowMessage createWorkflowMessage = message as CreateWorkflowMessage;
                    string name = createWorkflowMessage.WorkflowDefinition.Name;
                    int version = (int)createWorkflowMessage.WorkflowDefinition.Version;
                    string instanceName = BuildInstanceName(name, version);
                    if (Context.Child(instanceName) == ActorRefs.Nobody)
                    {
                        try
                        {
                            var actorProps = Context.DI().Props<WorkflowActor>();
                            //TODO add workflowDefinition for each actor

                            var workflowRef = Context.ActorOf(actorProps, instanceName);

                            _workflows.Add(name, new Dictionary<int, IActorRef>() { { version, workflowRef } });

                            Sender.Tell(new CreateWorkflowSucceededMessage(createWorkflowMessage, workflowRef));
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = string.Format("{0} failed in {1}", ActorId, typeof(CreateWorkflowMessage).Name);
                            Logger.Error(ex, errorMessage);
                            Sender.Tell(new CreateWorkflowFailedMessage(createWorkflowMessage, ex, errorMessage));
                        }
                    }
                    else
                    {
                        string errorMessage = string.Format("{0} received a {1} for a already existing Module", ActorId, typeof(CreateWorkflowMessage).Name);
                        Logger.Debug(errorMessage);
                        Sender.Tell(new CreateWorkflowFailedMessage(createWorkflowMessage, errorMessage));
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

        private Dictionary<string, Dictionary<int, IActorRef>> _workflows = new Dictionary<string, Dictionary<int, IActorRef>>();

        protected override VersionNumber ActorVersion
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Looks up an active Workflow child. If version is not supplied the highest version is returned
        /// </summary>
        /// <param name="workflowKey"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private IActorRef LookupWorkflow(KeyString workflowKey, VersionNumber version = null)
        {
            if (_workflows.TryGetValue(workflowKey, out Dictionary<int, IActorRef> versions))
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
