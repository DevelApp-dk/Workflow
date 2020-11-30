using Akka.Actor;
using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Messages
{
    public class LookupDataOwnerMessage
    {
        public LookupDataOwnerMessage(KeyString dataOwnerKey, SemanticVersionNumber dataOwnerVersion = null)
        {
            DataOwnerKey = dataOwnerKey;
            DataOwnerVersion = dataOwnerVersion;
        }

        public KeyString DataOwnerKey { get; }
        public SemanticVersionNumber DataOwnerVersion { get; }
    }

    public class LookupDataOwnerFailedMessage
    {
        public LookupDataOwnerFailedMessage(LookupDataOwnerMessage lookupDataOwnerMessage)
        {
            DataOwnerKey = lookupDataOwnerMessage.DataOwnerKey.Clone();
            DataOwnerVersion = lookupDataOwnerMessage.DataOwnerVersion.Clone();
        }

        public KeyString DataOwnerKey { get; }
        public SemanticVersionNumber DataOwnerVersion { get; }
    }

    public class LookupDataOwnerSucceededMessage
    {
        public LookupDataOwnerSucceededMessage(LookupDataOwnerMessage lookupDataOwnerMessage, IActorRef dataOwnerActorRef)
        {
            DataOwnerKey = lookupDataOwnerMessage.DataOwnerKey.Clone();
            DataOwnerVersion = lookupDataOwnerMessage.DataOwnerVersion.Clone();
            DataOwnerActorRef = dataOwnerActorRef;
        }

        public KeyString DataOwnerKey { get; }
        public SemanticVersionNumber DataOwnerVersion { get; }
        public IActorRef DataOwnerActorRef { get; }
    }
}
