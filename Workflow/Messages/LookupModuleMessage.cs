using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class LookupModuleMessage: LookupDataOwnerMessage
    {
        public LookupModuleMessage(KeyString dataOwnerKey, KeyString moduleKey):base(dataOwnerKey)
        {
            ModuleKey = moduleKey;
        }

        /// <summary>
        /// Returns the unique module key that is tried to look up
        /// </summary>
        public KeyString ModuleKey { get; }
    }
}
