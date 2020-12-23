using Akka.Actor;
using DevelApp.Utility.Model;
using DevelApp.Workflow.Interfaces;
using DevelApp.Workflow.Model;
using System;

namespace DevelApp.Workflow.Messages
{
    public class CreateSagaMessage:ISagaCRUDMessage
    {
        public CreateSagaMessage():this(Guid.NewGuid().ToString())
        { }

        public CreateSagaMessage(KeyString sagaKey)
        {
            SagaKey = sagaKey;
        }

        /// <summary>
        /// Returns the Saga unique key
        /// </summary>
        public KeyString SagaKey { get; }

        public CRUDMessageType CRUDMessageType
        {
            get
            {
                return CRUDMessageType.Create;
            }
        }
    }

    public class CreateSagaFailedMessage
    {
        public CreateSagaFailedMessage(CreateSagaMessage createSagaMessage, string errorMessage) : this(createSagaMessage, null, errorMessage)
        {
        }

        public CreateSagaFailedMessage(CreateSagaMessage createSagaMessage, Exception ex, string errorMessage)
        {
            SagaKey = (string)createSagaMessage.SagaKey;
            ErrorMessage = errorMessage;
            Exception = ex;
        }

        public KeyString SagaKey { get; }
        public string ErrorMessage { get; }
        public Exception Exception { get; }
    }

    public class CreateSagaSucceededMessage
    {
        public CreateSagaSucceededMessage(CreateSagaMessage createSagaMessage, IActorRef sagaActorRef)
        {
            SagaKey = (string)createSagaMessage.SagaKey;
            SagaActorRef = sagaActorRef;
        }

        public KeyString SagaKey { get; }
        public IActorRef SagaActorRef { get; }
    }

}
