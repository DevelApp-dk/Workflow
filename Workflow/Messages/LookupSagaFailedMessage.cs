using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class LookupSagaFailedMessage: LookupSagaMessage
    {
        public LookupSagaFailedMessage(KeyString dataOwnerKey, KeyString moduleKey, KeyString workflowKey, KeyString sagaKey) : base(dataOwnerKey, moduleKey, workflowKey, sagaKey)
        { }
    }
}
