using Akka.Actor;
using Default;
using DevelApp.Utility.Model;
using DevelApp.Workflow.Interfaces;
using DevelApp.Workflow.Model;
using System;

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
            WorkflowKey = createWorkflowMessage.WorkflowKey.Clone();
            WorkflowVersion = createWorkflowMessage.WorkflowVersion.Clone();
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
            WorkflowKey = createWorkflowMessage.WorkflowKey.Clone();
            WorkflowVersion = createWorkflowMessage.WorkflowVersion.Clone();
            WorkflowActorRef = workflowActorRef;
        }

        public KeyString WorkflowKey { get; }
        public SemanticVersionNumber WorkflowVersion { get; }
        public IActorRef WorkflowActorRef { get; }
    }
}
