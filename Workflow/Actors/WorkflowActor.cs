using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Actors
{
    /// <summary>
    /// Holds the specific workflow and is parent for all SagaActors
    /// </summary>
    public class WorkflowActor : AbstractPersistedWorkflowActor
    {
        protected override int Actor_Version => throw new NotImplementedException();

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
