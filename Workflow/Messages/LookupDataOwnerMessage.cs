using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class LookupDataOwnerMessage
    {
        public LookupDataOwnerMessage(KeyString dataOwnerKey)
        {
            DataOwnerKey = dataOwnerKey;
        }

        public KeyString DataOwnerKey { get; }
    }
}
