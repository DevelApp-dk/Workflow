using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class SagaMessage : ISagaMessage
    {
        public string SagaKey { get; internal set; }
    }

    public class SagaFailedMessage
    {
        public SagaFailedMessage(SagaMessage sagaMessage, string errorMessage) : this(sagaMessage, null, errorMessage)
        {
        }

        public SagaFailedMessage(SagaMessage sagaMessage, Exception ex, string errorMessage)
        {
            SagaKey = (string)sagaMessage.SagaKey;
            ErrorMessage = errorMessage;
            Exception = ex;
        }

        public KeyString SagaKey { get; }
        public string ErrorMessage { get; }
        public Exception Exception { get; }
    }

    public class SagaSucceededMessage
    {
        public SagaSucceededMessage(SagaMessage sagaMessage)
        {
            SagaKey = (string)sagaMessage.SagaKey;
        }

        public KeyString SagaKey { get; }
    }

}
