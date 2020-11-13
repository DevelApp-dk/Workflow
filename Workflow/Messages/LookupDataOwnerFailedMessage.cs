using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class LookupDataOwnerFailedMessage: LookupDataOwnerMessage
    {
        public LookupDataOwnerFailedMessage(KeyString dataOwnerKey): base(dataOwnerKey)
        {
        }
    }
}
