using Akka.Actor;
using Akka.Persistence;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Messages;
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
    public class ConfigurationActor : AbstractPersistedWorkflowActor<IWorkflowMessage, >
    {
        protected override VersionNumber ActorVersion
        {
            get
            {
                return 1;
            }
        }

        protected override void WorkflowMessageHandler(IWorkflowMessage message)
        {
            switch (message.MessageTypeName)
            {
                default:
                    Logger.Warning("{0} Did not handle received message [{1}] from [{2}]", ActorId, message.MessageTypeName, message.OriginalSender);
                    Sender.Tell(new WorkflowUnhandledMessage(message, Self.Path));
                    break;
            }
        }
    }
}
