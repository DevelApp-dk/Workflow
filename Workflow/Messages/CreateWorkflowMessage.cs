using Akka.Actor;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Interfaces;
using DevelApp.Workflow.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class CreateWorkflowMessage: IWorkflowCRUDMessage
    {
        public CreateWorkflowMessage(WorkflowDefinition workflowDefinition)
        {
            WorkflowDefinition = workflowDefinition;
        }

        /// <summary>
        /// Returns the Workflow unique key
        /// </summary>
        public KeyString WorkflowKey
        {
            get
            {
                return WorkflowDefinition.Name;
            }
        }

        public SemanticVersionNumber WorkflowVersion
        {
            get
            {
                return WorkflowDefinition.Version;
            }
        }

        /// <summary>
        /// Returns the Workflow definition
        /// </summary>
        public WorkflowDefinition WorkflowDefinition { get; }

        public CRUDMessageType CRUDMessageType
        {
            get
            {
                return CRUDMessageType.Create;
            }
        }
    }

    public class CreateWorkflowFailedMessage
    {
        public CreateWorkflowFailedMessage(CreateWorkflowMessage createWorkflowMessage, string errorMessage) : this(createWorkflowMessage, null, errorMessage)
        {
        }

        public CreateWorkflowFailedMessage(CreateWorkflowMessage createWorkflowMessage, Exception ex, string errorMessage)
        {
            WorkflowKey = (string)createWorkflowMessage.WorkflowKey;
            WorkflowVersion = (int)createWorkflowMessage.WorkflowVersion;
            ErrorMessage = errorMessage;
            Exception = ex;
        }

        public KeyString WorkflowKey { get; }
        public SemanticVersionNumber WorkflowVersion { get; }
        public string ErrorMessage { get; }
        public Exception Exception { get; }
    }

    public class CreateWorkflowSucceededMessage
    {
        public CreateWorkflowSucceededMessage(CreateWorkflowMessage createWorkflowMessage, IActorRef workflowActorRef)
        {
            WorkflowKey = (string)createWorkflowMessage.WorkflowKey;
            WorkflowVersion = (int)createWorkflowMessage.WorkflowVersion;
            WorkflowActorRef = workflowActorRef;
        }

        public KeyString WorkflowKey { get; }
        public SemanticVersionNumber WorkflowVersion { get; }
        public IActorRef WorkflowActorRef { get; }
    }
}
