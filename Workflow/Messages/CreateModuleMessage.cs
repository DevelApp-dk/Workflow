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

        public VersionNumber ModuleVersion
        {
            get
            {
                return ModuleDefinition.Version;
            }
        }


        /// <summary>
        /// Returns the module definition
        /// </summary>
        public ModuleDefinition ModuleDefinition { get; }
    }

    public class CreateDataOwnerFailedMessage
    {
        public CreateModuleFailedMessage(CreateModuleMessage createModuleMessage, string errorMessage) : this(createModuleMessage, null, errorMessage)
        {
        }

        public CreatemoduleFailedMessage(CreateModuleMessage createModuleMessage, Exception ex, string errorMessage)
        {
            ModuleKey = (string)createModuleMessage.ModuleKey;
            ModuleVersion = (int)createModuleMessage.ModuleVersion;
            ErrorMessage = errorMessage;
            Exception = ex;
        }

        public KeyString ModuleKey { get; }
        public VersionNumber ModuleVersion { get; }
        public string ErrorMessage { get; }
        public Exception Exception { get; }
    }

    public class CreateModuleSucceededMessage
    {
        public CreateModuleSucceededMessage(CreateModuleMessage createModuleMessage, IActorRef moduleActorRef)
        {
            ModuleKey = (string)createModuleMessage.ModuleKey;
            ModuleVersion = (int)createModuleMessage.ModuleVersion;
            ModuleActorRef = moduleActorRef;
        }

        public KeyString ModuleKey { get; }
        public VersionNumber ModuleVersion { get; }
        public IActorRef ModuleActorRef { get; }
    }
}
