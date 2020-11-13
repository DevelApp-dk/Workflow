using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    public class CreateWorkflowMessage
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

        /// <summary>
        /// Returns the Workflow definition
        /// </summary>
        public WorkflowDefinition WorkflowDefinition { get; }
    }
}
