using Akka.Actor;
using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Messages
{
    public class LookupModuleMessage
    {
        public LookupModuleMessage(KeyString dataOwnerKey, KeyString moduleKey, SemanticVersionNumber dataOwnerVersion = null, SemanticVersionNumber moduleVersion = null)
        {
            DataOwnerKey = dataOwnerKey;
            DataOwnerVersion = dataOwnerVersion;
            ModuleKey = moduleKey;
            ModuleVersion = moduleVersion;
        }

        public KeyString DataOwnerKey { get; }
        public SemanticVersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public SemanticVersionNumber ModuleVersion { get; }
    }

    public class LookupModuleFailedMessage
    {
        public LookupModuleFailedMessage(LookupModuleMessage lookupModuleMessage)
        {
            DataOwnerKey = lookupModuleMessage.DataOwnerKey.Clone();
            DataOwnerVersion = lookupModuleMessage.DataOwnerVersion.Clone();
            ModuleKey = lookupModuleMessage.ModuleKey.Clone();
            ModuleVersion = lookupModuleMessage.ModuleVersion.Clone();
        }

        public KeyString DataOwnerKey { get; }
        public SemanticVersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public SemanticVersionNumber ModuleVersion { get; }
    }

    public class LookupModuleSucceededMessage
    {
        public LookupModuleSucceededMessage(LookupModuleMessage lookupModuleMessage, IActorRef moduleActorRef)
        {
            DataOwnerKey = lookupModuleMessage.DataOwnerKey.Clone();
            DataOwnerVersion = lookupModuleMessage.DataOwnerVersion.Clone();
            ModuleKey = lookupModuleMessage.ModuleKey.Clone();
            ModuleVersion = lookupModuleMessage.ModuleVersion.Clone();
            ModuleActorRef = moduleActorRef;
        }

        public KeyString DataOwnerKey { get; }
        public SemanticVersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public SemanticVersionNumber ModuleVersion { get; }
        public IActorRef ModuleActorRef { get; }
    }
}
