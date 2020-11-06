using Akka.Actor;
using Akka.Persistence;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DevelApp.Workflow.Actors
{
    /// <summary>
    /// Holds a lookup for configuration
    /// </summary>
    public class ConfigurationActor : AbstractPersistedWorkflowActor
    {
        protected override int ActorVersion
        {
            get
            {
                return 1;
            }
        }

        protected override void RecoverPersistedWorkflowDataHandler(JsonValue message)
        {
            throw new NotImplementedException();
        }

        protected override void WorkflowMessageHandler(JsonValue message)
        {
            throw new NotImplementedException();
        }
    }
}
