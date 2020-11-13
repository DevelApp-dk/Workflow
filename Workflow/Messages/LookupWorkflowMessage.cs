using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class LookupWorkflowMessage:LookupModuleMessage
    {
        public LookupWorkflowMessage(KeyString dataOwnerKey, KeyString moduleKey, KeyString workflowKey):base(dataOwnerKey, moduleKey)
        {
            WorkflowKey = workflowKey;
        }

        public KeyString WorkflowKey { get; }
    }
}
