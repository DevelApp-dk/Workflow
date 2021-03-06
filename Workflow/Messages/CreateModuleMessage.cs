﻿using Akka.Actor;
using Default;
using DevelApp.Utility.Model;
using DevelApp.Workflow.Interfaces;
using DevelApp.Workflow.Model;
using System;

namespace DevelApp.Workflow.Messages
{
    public class CreateModuleMessage:IModuleCRUDMessage
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

        public SemanticVersionNumber ModuleVersion
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

        public CRUDMessageType CRUDMessageType
        {
            get
            {
                return CRUDMessageType.Create;
            }
        }
    }

    public class CreateModuleFailedMessage
    {
        public CreateModuleFailedMessage(CreateModuleMessage createModuleMessage, string errorMessage) : this(createModuleMessage, null, errorMessage)
        {
        }

        public CreateModuleFailedMessage(CreateModuleMessage createModuleMessage, Exception ex, string errorMessage)
        {
            ModuleKey = createModuleMessage.ModuleKey.Clone();
            ModuleVersion = createModuleMessage.ModuleVersion.Clone();
            ErrorMessage = errorMessage;
            Exception = ex;
        }

        public KeyString ModuleKey { get; }
        public SemanticVersionNumber ModuleVersion { get; }
        public string ErrorMessage { get; }
        public Exception Exception { get; }
    }

    public class CreateModuleSucceededMessage
    {
        public CreateModuleSucceededMessage(CreateModuleMessage createModuleMessage, IActorRef moduleActorRef)
        {
            ModuleKey = createModuleMessage.ModuleKey.Clone();
            ModuleVersion = createModuleMessage.ModuleVersion.Clone();
            ModuleActorRef = moduleActorRef;
        }

        public KeyString ModuleKey { get; }
        public SemanticVersionNumber ModuleVersion { get; }
        public IActorRef ModuleActorRef { get; }
    }
}
