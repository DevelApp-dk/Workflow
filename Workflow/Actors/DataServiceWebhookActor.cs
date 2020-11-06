using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Actors
{
    public class DataServiceWebhookActor : AbstractWorkflowActor
    {
        protected override int ActorVersion
        {
            get
            {
                return 1;
            }
        }

        protected override void WorkflowMessageHandler(JsonValue message)
        {
            throw new NotImplementedException();
        }
    }
}
