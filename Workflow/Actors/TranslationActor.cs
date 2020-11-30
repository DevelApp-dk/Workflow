using Akka.Actor;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.AbstractActors;
using DevelApp.Workflow.Core.Messages;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.GeneratedModel;
using DevelApp.Workflow.Messages;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Actors
{
    public class TranslationActor : AbstractPersistedWorkflowActor<IWorkflowMessage, Dictionary<string,Translation>>
    {
        protected override void DoLastActionsAfterRecover()
        {
            throw new NotImplementedException();
        }

        protected override void GroupFinishedMessageHandler(GroupFinishedMessage message)
        {
            throw new NotImplementedException();
        }

        protected override void RecoverPersistedWorkflowDataHandler(IWorkflowMessage dataItem)
        {
            throw new NotImplementedException();
        }

        protected override void WorkflowMessageHandler(IWorkflowMessage message)
        {
            switch (message.MessageTypeName)
            {
                default:
                    Logger.Warning("{0} Did not handle received message [{1}] from [{2}]", ActorId, message.MessageTypeName, Sender.Path);
                    if (!Sender.IsNobody() && !message.IsReply)
                    {
                        Sender.Tell((message as WorkflowMessage).GetWorkflowUnhandledMessage("Message Type Not Implemented", Self.Path));
                    }
                    break;
            }
        }
    }
}
