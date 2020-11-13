using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class LookupSagaMessage:LookupWorkflowMessage
    {
        public LookupSagaMessage(KeyString dataOwnerKey, KeyString moduleKey, KeyString workflowKey, KeyString sagaKey) : base(dataOwnerKey, moduleKey, workflowKey)
        {
            SagaKey = sagaKey;
        }

        public KeyString SagaKey { get; }
    }
}
