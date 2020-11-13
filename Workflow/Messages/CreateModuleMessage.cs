using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class CreateModuleMessage
    {
        public CreateModuleMessage(ModuleDefinition moduleDefinition)
        {
            ModuleDefinition = moduleDefinition;
        }

        /// <summary>
        /// Returns the Module unique key
        /// </summary>
        public KeyString ModuleKey
        {
            get
            {
                return ModuleDefinition.Name;
            }
        }

        /// <summary>
        /// Returns the module definition
        /// </summary>
        public ModuleDefinition ModuleDefinition { get; }
    }
}
