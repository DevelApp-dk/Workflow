using Akka.Actor;
using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class LookupWorkflowMessage
    {
        public LookupWorkflowMessage(KeyString dataOwnerKey, KeyString moduleKey, KeyString workflowKey, VersionNumber dataOwnerVersion = null, VersionNumber moduleVersion = null, VersionNumber workflowVersion = null)
        {
            DataOwnerKey = dataOwnerKey;
            DataOwnerVersion = dataOwnerVersion;
            ModuleKey = moduleKey;
            ModuleVersion = moduleVersion;
            WorkflowKey = workflowKey;
            WorkflowVersion = workflowVersion;
        }

        public KeyString DataOwnerKey { get; }
        public VersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public VersionNumber ModuleVersion { get; }
        public KeyString WorkflowKey { get; }
        public VersionNumber WorkflowVersion { get; }
    }

    public class LookupWorkflowFailedMessage
    {
        public LookupWorkflowFailedMessage(LookupWorkflowMessage lookupWorkflowMessage)
        {
            DataOwnerKey = (string)lookupWorkflowMessage.DataOwnerKey;
            DataOwnerVersion = (int)lookupWorkflowMessage.DataOwnerVersion;
            ModuleKey = (string)lookupWorkflowMessage.ModuleKey;
            ModuleVersion = (int)lookupWorkflowMessage.ModuleVersion;
            WorkflowKey = (string)lookupWorkflowMessage.WorkflowKey;
            WorkflowVersion = (int)lookupWorkflowMessage.WorkflowVersion;
        }

        public KeyString DataOwnerKey { get; }
        public VersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public VersionNumber ModuleVersion { get; }
        public KeyString WorkflowKey { get; }
        public VersionNumber WorkflowVersion { get; }
    }

    public class LookupWorkflowSucceededMessage
    {
        public LookupWorkflowSucceededMessage(LookupWorkflowMessage lookupWorkflowMessage, IActorRef workflowActorRef)
        {
            DataOwnerKey = (string)lookupWorkflowMessage.DataOwnerKey;
            DataOwnerVersion = (int)lookupWorkflowMessage.DataOwnerVersion;
            ModuleKey = (string)lookupWorkflowMessage.ModuleKey;
            ModuleVersion = (int)lookupWorkflowMessage.ModuleVersion;
            WorkflowKey = (string)lookupWorkflowMessage.WorkflowKey;
            WorkflowVersion = (int)lookupWorkflowMessage.WorkflowVersion;
            WorkflowActorRef = workflowActorRef;
        }

        public KeyString DataOwnerKey { get; }
        public VersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public VersionNumber ModuleVersion { get; }
        public KeyString WorkflowKey { get; }
        public VersionNumber WorkflowVersion { get; }
        public IActorRef WorkflowActorRef { get; }
    }
}
