using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Actors
{
    /// <summary>
    /// Singleton parent of potentially multiple DataOwnerActor
    /// </summary>
    public class DataOwnerCoordinatorActor : AbstractPersistedWorkflowActor
    {
        protected override int Actor_Version
        {
            get
            {
                return 1;
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
