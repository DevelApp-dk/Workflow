using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class CreateSagaMessage
    {
        public CreateSagaMessage(KeyString sagaKey)
        {
            SagaKey = sagaKey;
        }

        /// <summary>
        /// Returns the Saga unique key
        /// </summary>
        public KeyString SagaKey { get; }
    }
}
