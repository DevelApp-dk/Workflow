using Akka.Actor;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Messages;
using Manatee.Json;
using System;

namespace DevelApp.Workflow.Actors
{
    /// <summary>
    /// Represents the individual sagas
    /// </summary>
    public class SagaActor : AbstractPersistedWorkflowActor<JsonValue>
    {
        public Guid SagaId { get; }
        private Model.Workflow _workflow;
        private ISagaStepBehaviorFactory _behaviorFactory;

        public SagaActor(Model.Workflow workflow, ISagaStepBehaviorFactory behaviorFactory, Guid sagaId = default)
        {
            if(sagaId == default)
            {
                SagaId = Guid.NewGuid();
            }
            else
            {
                SagaId = sagaId;
            }
            _workflow = workflow;
            _behaviorFactory = behaviorFactory;
        }

        protected override VersionNumber ActorVersion
        {
            get
            {
                return 1;
            }
        }

        public override string PersistenceId
        {
            get
            {
                return SagaId.ToString();
            }
        }

        protected override void RecoverPersistedWorkflowDataHandler(JsonValue data)
        {
            throw new NotImplementedException();
        }

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
    }
}
