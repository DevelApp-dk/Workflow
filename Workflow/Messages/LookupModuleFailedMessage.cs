using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class LookupModuleFailedMessage:LookupModuleMessage
    {
        public LookupModuleFailedMessage(KeyString dataOwnerKey, KeyString moduleKey) : base(dataOwnerKey, moduleKey)
        {
        }
    }
}
