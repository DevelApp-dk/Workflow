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
}
