using Akka.Actor;
using Akka.Monitoring;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Interfaces;
using DevelApp.Workflow.Messages;
using DevelApp.Workflow.Model;
using Manatee.Json;
using System;
using System.Collections.Generic;

namespace DevelApp.Workflow.Actors
{
    /// <summary>
    /// Represents the individual sagas
    /// </summary>
    public class SagaActor : AbstractPersistedWorkflowActor<ISagaMessage, List<ISagaMessage>>
    {
        private Model.Workflow _workflow;

        public SagaActor(Model.Workflow workflow, KeyString sagaKey)
        {
            SagaKey = sagaKey;
            _workflow = workflow;
            Saga = new Saga();

            Command<ISagaMessage>(message =>
            {
                Context.IncrementMessagesReceived();
                PersistWorkflowData(message);
                SagaMessageHandler(message);
            });
        }

        protected Saga Saga { get; }

        protected override VersionNumber ActorVersion
        {
            get
            {
                return 1;
            }
        }

        protected override string ActorId
        {
            get
            {
                return SagaKey;
            }
        }

        protected KeyString SagaKey { get; }

        protected override void WorkflowMessageHandler(WorkflowMessage message)
        {
            switch(message.MessageTypeName)
            {
                default:
                    Logger.Warning("{0} Did not handle received message [{1}] from [{2}]", ActorId, message.MessageTypeName, message.OriginalSender);
                    Sender.Tell(new WorkflowUnhandledMessage(message, Self.Path));
                    break;
            }
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
            //Pickup where last message died
            ISagaMessage latestSagaMessage = PersistedCollection[PersistedCollection.Count - 1];
            SagaMessageHandler(latestSagaMessage);
        }

        protected override void RecoverPersistedWorkflowDataHandler(ISagaMessage dataItem)
        {
            SagaMessageHandler(dataItem, changeStateOnly:true);
        }

        private void SagaMessageHandler(ISagaMessage sagaMessage, bool changeStateOnly = false)
        {
            throw new NotImplementedException();
        }
    }
}
