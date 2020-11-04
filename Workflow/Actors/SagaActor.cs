using Manatee.Json;
using System;

namespace Workflow.Actors
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
    }
}
