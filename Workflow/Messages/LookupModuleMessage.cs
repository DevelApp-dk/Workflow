using Akka.Actor;
using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Messages
{
    public class LookupModuleMessage
    {
        public LookupModuleMessage(KeyString dataOwnerKey, KeyString moduleKey, VersionNumber dataOwnerVersion = null, VersionNumber moduleVersion = null)
        {
            DataOwnerKey = dataOwnerKey;
            DataOwnerVersion = dataOwnerVersion;
            ModuleKey = moduleKey;
            ModuleVersion = moduleVersion;
        }

        public KeyString DataOwnerKey { get; }
        public VersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public VersionNumber ModuleVersion { get; }
    }

    public class LookupModuleFailedMessage
    {
        public LookupModuleFailedMessage(LookupModuleMessage lookupModuleMessage)
        {
            DataOwnerKey = (string)lookupModuleMessage.DataOwnerKey;
            DataOwnerVersion = (int)lookupModuleMessage.DataOwnerVersion;
            ModuleKey = (string)lookupModuleMessage.ModuleKey;
            ModuleVersion = (int)lookupModuleMessage.ModuleVersion;
        }

        public KeyString DataOwnerKey { get; }
        public VersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public VersionNumber ModuleVersion { get; }
    }

    public class LookupModuleSucceededMessage
    {
        public LookupModuleSucceededMessage(LookupModuleMessage lookupModuleMessage, IActorRef moduleActorRef)
        {
            DataOwnerKey = (string)lookupModuleMessage.DataOwnerKey;
            DataOwnerVersion = (int)lookupModuleMessage.DataOwnerVersion;
            ModuleKey = (string)lookupModuleMessage.ModuleKey;
            ModuleVersion = (int)lookupModuleMessage.ModuleVersion;
            ModuleActorRef = moduleActorRef;
        }

        public KeyString DataOwnerKey { get; }
        public VersionNumber DataOwnerVersion { get; }
        public KeyString ModuleKey { get; }
        public VersionNumber ModuleVersion { get; }
        public IActorRef ModuleActorRef { get; }
    }
}
