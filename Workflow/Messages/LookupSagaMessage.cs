using Akka.Actor;
using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Messages
{
    public class LookupSagaMessage
    {
        public LookupSagaMessage(KeyString dataOwnerKey, KeyString moduleKey, KeyString workflowKey, KeyString sagaKey, VersionNumber dataOwnerVersion = null, VersionNumber moduleVersion = null, VersionNumber workflowVersion = null)
        {
            DataOwnerKey = dataOwnerKey;
            DataOwnerVersion = dataOwnerVersion;
            ModuleKey = moduleKey;
            ModuleVersion = moduleVersion;
            WorkflowKey = workflowKey;
            WorkflowVersion = workflowVersion;
            SagaKey = sagaKey;
        }

        public KeyString DataOwnerKey { get; }
        public VersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public VersionNumber ModuleVersion { get; }
        public KeyString WorkflowKey { get; }
        public VersionNumber WorkflowVersion { get; }
        public KeyString SagaKey { get; }
    }

    public class LookupSagaFailedMessage
    {
        public LookupSagaFailedMessage(LookupSagaMessage lookupSagaMessage)
        {
            DataOwnerKey = (string)lookupSagaMessage.DataOwnerKey;
            DataOwnerVersion = (int)lookupSagaMessage.DataOwnerVersion;
            ModuleKey = (string)lookupSagaMessage.ModuleKey;
            ModuleVersion = (int)lookupSagaMessage.ModuleVersion;
            WorkflowKey = (string)lookupSagaMessage.WorkflowKey;
            WorkflowVersion = (int)lookupSagaMessage.WorkflowVersion;
            SagaKey = (string)lookupSagaMessage.SagaKey;
        }

        public KeyString DataOwnerKey { get; }
        public VersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public VersionNumber ModuleVersion { get; }
        public KeyString WorkflowKey { get; }
        public VersionNumber WorkflowVersion { get; }
        public KeyString SagaKey { get; }
    }

    public class LookupSagaSucceededMessage
    {
        public LookupSagaSucceededMessage(LookupSagaMessage lookupSagaMessage, IActorRef sagaActorRef)
        {
            DataOwnerKey = (string)lookupSagaMessage.DataOwnerKey;
            DataOwnerVersion = (int)lookupSagaMessage.DataOwnerVersion;
            ModuleKey = (string)lookupSagaMessage.ModuleKey;
            ModuleVersion = (int)lookupSagaMessage.ModuleVersion;
            WorkflowKey = (string)lookupSagaMessage.WorkflowKey;
            WorkflowVersion = (int)lookupSagaMessage.WorkflowVersion;
            SagaKey = (string)lookupSagaMessage.SagaKey;
            SagaActorRef = sagaActorRef;
        }

        public KeyString DataOwnerKey { get; }
        public VersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public VersionNumber ModuleVersion { get; }
        public KeyString WorkflowKey { get; }
        public VersionNumber WorkflowVersion { get; }
        public KeyString SagaKey { get; }
        public IActorRef SagaActorRef { get; }
    }
}
