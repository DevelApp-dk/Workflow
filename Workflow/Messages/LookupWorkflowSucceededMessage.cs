using Akka.Actor;
using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Messages
{
    public class LookupWorkflowSucceededMessage:LookupWorkflowMessage
    {
        public LookupWorkflowSucceededMessage(KeyString dataOwnerKey, KeyString moduleKey, KeyString workflowKey, IActorRef workflowActorRef) : base(dataOwnerKey, moduleKey, workflowKey)
        {
            WorkflowActorRef = workflowActorRef;
        }

        public IActorRef WorkflowActorRef { get; }
    }
}
