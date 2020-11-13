using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Messages
{
    public class LookupWorkflowFailedMessage: LookupWorkflowMessage
    {
        public LookupWorkflowFailedMessage(KeyString dataOwnerKey, KeyString moduleKey, KeyString workflowKey) : base(dataOwnerKey, moduleKey, workflowKey)
        { }
    }
}
