using Akka.Actor;
using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class LookupDataOwnerSucceededMessage:LookupDataOwnerMessage
    {
        public LookupDataOwnerSucceededMessage(KeyString dataOwnerKey, IActorRef dataOwnerActorRef):base(dataOwnerKey)
        {
            DataOwnerActorRef = dataOwnerActorRef;
        }

        public IActorRef DataOwnerActorRef { get; }
    }
}
