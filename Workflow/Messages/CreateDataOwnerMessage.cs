using Akka.Actor;
using Default;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Interfaces;
using DevelApp.Workflow.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class CreateDataOwnerMessage: IDataOwnerCRUDMessage
    {
        public CreateDataOwnerMessage(DataOwnerDefinition dataOwnerDefinition)
        {
            DataOwnerDefinition = dataOwnerDefinition;
        }

        /// <summary>
        /// Returns the DataOwner unique key
        /// </summary>
        public KeyString DataOwnerKey 
        { 
            get
            {
                return DataOwnerDefinition.Name;
            }
        }

        public SemanticVersionNumber DataOwnerVersion
        {
            get
            {
                return DataOwnerDefinition.Version;
            }
        }

        /// <summary>
        /// Returns the data owner definition
        /// </summary>
        public DataOwnerDefinition DataOwnerDefinition { get; }

        public CRUDMessageType CRUDMessageType
        {
            get
            {
                return CRUDMessageType.Create;
            }
        }
    }

    public class CreateDataOwnerFailedMessage
    {
        public CreateDataOwnerFailedMessage(CreateDataOwnerMessage createDataOwnerMessage, string errorMessage):this(createDataOwnerMessage, null, errorMessage)
        {
        }

        public CreateDataOwnerFailedMessage(CreateDataOwnerMessage createDataOwnerMessage, Exception ex, string errorMessage)
        {
            DataOwnerKey = createDataOwnerMessage.DataOwnerKey.Clone();
            DataOwnerVersion = createDataOwnerMessage.DataOwnerVersion.Clone();
            ErrorMessage = errorMessage;
            Exception = ex;
        }

        public KeyString DataOwnerKey { get; }
        public SemanticVersionNumber DataOwnerVersion { get; }
        public string ErrorMessage { get; }
        public Exception Exception { get; }
    }

    public class CreateDataOwnerSucceededMessage
    {
        public CreateDataOwnerSucceededMessage(CreateDataOwnerMessage createDataOwnerMessage, IActorRef dataOwnerActorRef)
        {
            DataOwnerKey = createDataOwnerMessage.DataOwnerKey.Clone();
            DataOwnerVersion = createDataOwnerMessage.DataOwnerVersion.Clone();
            DataOwnerActorRef = dataOwnerActorRef;
        }

        public KeyString DataOwnerKey { get; }
        public SemanticVersionNumber DataOwnerVersion { get; }
        public IActorRef DataOwnerActorRef { get; }
    }
}
