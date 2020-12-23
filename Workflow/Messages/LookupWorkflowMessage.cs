using Akka.Actor;
using DevelApp.Utility.Model;

namespace DevelApp.Workflow.Messages
{
    public class LookupWorkflowMessage
    {
        public LookupWorkflowMessage(KeyString dataOwnerKey, KeyString moduleKey, KeyString workflowKey, SemanticVersionNumber dataOwnerVersion = null, SemanticVersionNumber moduleVersion = null, SemanticVersionNumber workflowVersion = null)
        {
            DataOwnerKey = dataOwnerKey;
            DataOwnerVersion = dataOwnerVersion;
            ModuleKey = moduleKey;
            ModuleVersion = moduleVersion;
            WorkflowKey = workflowKey;
            WorkflowVersion = workflowVersion;
        }

        public KeyString DataOwnerKey { get; }
        public SemanticVersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public SemanticVersionNumber ModuleVersion { get; }
        public KeyString WorkflowKey { get; }
        public SemanticVersionNumber WorkflowVersion { get; }
    }

    public class LookupWorkflowFailedMessage
    {
        public LookupWorkflowFailedMessage(LookupWorkflowMessage lookupWorkflowMessage)
        {
            DataOwnerKey = lookupWorkflowMessage.DataOwnerKey.Clone();
            DataOwnerVersion = lookupWorkflowMessage.DataOwnerVersion.Clone();
            ModuleKey = lookupWorkflowMessage.ModuleKey.Clone();
            ModuleVersion = lookupWorkflowMessage.ModuleVersion.Clone();
            WorkflowKey = lookupWorkflowMessage.WorkflowKey.Clone();
            WorkflowVersion = lookupWorkflowMessage.WorkflowVersion.Clone();
        }

        public KeyString DataOwnerKey { get; }
        public SemanticVersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public SemanticVersionNumber ModuleVersion { get; }
        public KeyString WorkflowKey { get; }
        public SemanticVersionNumber WorkflowVersion { get; }
    }

    public class LookupWorkflowSucceededMessage
    {
        public LookupWorkflowSucceededMessage(LookupWorkflowMessage lookupWorkflowMessage, IActorRef workflowActorRef)
        {
            DataOwnerKey = lookupWorkflowMessage.DataOwnerKey.Clone();
            DataOwnerVersion = lookupWorkflowMessage.DataOwnerVersion.Clone();
            ModuleKey = lookupWorkflowMessage.ModuleKey.Clone();
            ModuleVersion = lookupWorkflowMessage.ModuleVersion.Clone();
            WorkflowKey = lookupWorkflowMessage.WorkflowKey.Clone();
            WorkflowVersion = lookupWorkflowMessage.WorkflowVersion.Clone();
            WorkflowActorRef = workflowActorRef;
        }

        public KeyString DataOwnerKey { get; }
        public SemanticVersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public SemanticVersionNumber ModuleVersion { get; }
        public KeyString WorkflowKey { get; }
        public SemanticVersionNumber WorkflowVersion { get; }
        public IActorRef WorkflowActorRef { get; }
    }
}
