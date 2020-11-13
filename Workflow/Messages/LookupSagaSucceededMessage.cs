using Akka.Actor;
using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class LookupSagaSucceededMessage:LookupSagaMessage
    {
        public LookupSagaSucceededMessage(KeyString dataOwnerKey, KeyString moduleKey, KeyString workflowKey, KeyString sagaKey, IActorRef sagaActorRef) : base(dataOwnerKey, moduleKey, workflowKey, sagaKey)
        {
            SagaActorRef = sagaActorRef;
        }

        public IActorRef SagaActorRef { get; }
    }
}
