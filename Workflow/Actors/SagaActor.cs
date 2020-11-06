using Akka.Actor;
using Manatee.Json;
using System;

namespace DevelApp.Workflow.Actors
{
    /// <summary>
    /// Represents the individual sagas
    /// </summary>
    public class SagaActor : AbstractPersistedWorkflowActor
    {
        public Guid SagaId { get; }

        public SagaActor(Guid sagaId = default)
        {
            if(sagaId == default)
            {
                SagaId = Guid.NewGuid();
            }
            else
            {
                SagaId = sagaId;
            }
        }

        protected override int ActorVersion
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

        protected override void WorkflowMessageHandler(JsonValue message)
        {
            throw new NotImplementedException();
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
