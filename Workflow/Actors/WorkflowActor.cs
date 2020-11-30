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
    /// Holds the specific workflow and is parent for all SagaActors
    /// </summary>
    public class WorkflowActor : AbstractPersistedWorkflowActor<ISagaCRUDMessage, Dictionary<string, ISagaCRUDMessage>>
    {
        public WorkflowActor(WorkflowDefinition workflowDefinition, ISagaStepBehaviorFactory sagaStepBehaviorFactory)
        {
            //TODO check that sagaStepBehaviorFactory contains behaviors defined in workflowdefinition
            Workflow = new Model.Workflow(this, workflowDefinition, sagaStepBehaviorFactory);
            _sagaStepBehaviorFactory = sagaStepBehaviorFactory;

            Command<ISagaCRUDMessage>(message =>
            {
                Context.IncrementMessagesReceived();
                PersistWorkflowData(message);
                SagaCRUDMessageHandler(message);
            });

            Command<LookupSagaMessage>(message =>
            {
                Context.IncrementMessagesReceived();
                LookupSagaMessageHandler(message);
            });

            Command<ListAllSagasMessage>(message =>
            {
                Context.IncrementMessagesReceived();
                ListAllSagasMessageHandler(message);
            });

            #region Ignored Messages

            #endregion
        }

        private Workflow.Model.Workflow Workflow { get; }

        #region Handlers

        private void LookupSagaMessageHandler(LookupSagaMessage message)
        {
            IActorRef actorRef = LookupSaga(message.SagaKey);
            if (actorRef == ActorRefs.Nobody)
            {
                Sender.Tell(new LookupSagaFailedMessage(message));
            }
            else
            {
                Sender.Tell(new LookupSagaSucceededMessage(message, actorRef));
            }
        }

        private void ListAllSagasMessageHandler(ListAllSagasMessage message)
        {
            List<(KeyString, ReadOnlyCollection<SemanticVersionNumber>)> sagas = new List<(KeyString, ReadOnlyCollection<SemanticVersionNumber>)>();

            foreach (var sagaPair in _sagas)
            {
                List<SemanticVersionNumber> versions = new List<SemanticVersionNumber>();
                foreach (var version in sagaPair.Value.Keys)
                {
                    versions.Add(version);
                }
                sagas.Add((sagaPair.Key, versions.AsReadOnly()));
            }
            Sender.Tell(new ListAllSagasSucceededMessage(sagas.AsReadOnly()));
        }

        protected override void RecoverPersistedWorkflowDataHandler(ISagaCRUDMessage data)
        {
            SagaCRUDMessageHandler(data);
        }

        private void SagaCRUDMessageHandler(ISagaCRUDMessage message)
        {
            switch (message.CRUDMessageType)
            {
                case Model.CRUDMessageType.Create:
                    CreateSagaMessage createSagaMessage = message as CreateSagaMessage;
                    string name = createSagaMessage.SagaKey;
                    string instanceName = name;
                    if (Context.Child(instanceName) == ActorRefs.Nobody)
                    {
                        try
                        {
                            var actorProps = Context.DI().Props<SagaActor>();

                            var sagaRef = Context.ActorOf(actorProps, instanceName);

                            _sagas.Add(name, sagaRef);

                            Sender.Tell(new CreateSagaSucceededMessage(createSagaMessage, sagaRef));
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = string.Format("{0} failed in {1}", ActorId, typeof(CreateSagaMessage).Name);
                            Logger.Error(ex, errorMessage);
                            Sender.Tell(new CreateSagaFailedMessage(createSagaMessage, ex, errorMessage));
                        }
                    }
                    else
                    {
                        string errorMessage = string.Format("{0} received a {1} for a already existing Saga", ActorId, typeof(CreateSagaMessage).Name);
                        Logger.Debug(errorMessage);
                        Sender.Tell(new CreateSagaFailedMessage(createSagaMessage, errorMessage));
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

        //TODO handle dormant state actors
        private Dictionary<string, IActorRef> _sagas = new Dictionary<string, IActorRef>();

        //TODO setup subscription to observable states
        private ISagaStepBehaviorFactory _sagaStepBehaviorFactory;

        /// <summary>
        /// Looks up a possibly dormant saga child.
        /// </summary>
        /// <param name="workflowKey"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private IActorRef LookupSaga(KeyString sagaKey)
        {
            if (_sagas.TryGetValue(sagaKey, out IActorRef sagaActorRef))
            {
                return sagaActorRef;
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
