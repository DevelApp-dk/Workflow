using Akka.Actor;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Messages;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Actors
{
    public class DataServiceWebhookActor : AbstractWorkflowActor
    {
        protected override void WorkflowMessageHandler(WorkflowMessage message)
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
