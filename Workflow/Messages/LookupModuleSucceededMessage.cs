using Akka.Actor;
using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Messages
{
    public class LookupModuleSucceededMessage:LookupModuleMessage
    {
        public LookupModuleSucceededMessage(KeyString dataOwnerKey, KeyString moduleKey, IActorRef moduleActorRef) : base(dataOwnerKey, moduleKey)
        {
            ModuleActorRef = moduleActorRef;
        }

        public IActorRef ModuleActorRef { get; }
    }
}
