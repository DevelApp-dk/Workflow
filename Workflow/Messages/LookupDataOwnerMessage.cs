using Akka.Actor;
using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Messages
{
    public class LookupDataOwnerMessage
    {
        public LookupDataOwnerMessage(KeyString dataOwnerKey, VersionNumber dataOwnerVersion = null)
        {
            DataOwnerKey = dataOwnerKey;
            DataOwnerVersion = dataOwnerVersion;
        }

        public KeyString DataOwnerKey { get; }
        public VersionNumber DataOwnerVersion { get; }
    }

    public class LookupDataOwnerFailedMessage
    {
        public LookupDataOwnerFailedMessage(LookupDataOwnerMessage lookupDataOwnerMessage)
        {
            DataOwnerKey = (string)lookupDataOwnerMessage.DataOwnerKey;
            DataOwnerVersion = (int)lookupDataOwnerMessage.DataOwnerVersion;
        }

        public KeyString DataOwnerKey { get; }
        public VersionNumber DataOwnerVersion { get; }
    }

    public class LookupDataOwnerSucceededMessage
    {
        public LookupDataOwnerSucceededMessage(LookupDataOwnerMessage lookupDataOwnerMessage, IActorRef dataOwnerActorRef)
        {
            DataOwnerKey = (string)lookupDataOwnerMessage.DataOwnerKey;
            DataOwnerVersion = (int)lookupDataOwnerMessage.DataOwnerVersion;
            DataOwnerActorRef = dataOwnerActorRef;
        }

        public KeyString DataOwnerKey { get; }
        public VersionNumber DataOwnerVersion { get; }
        public IActorRef DataOwnerActorRef { get; }
    }
}
