using DevelApp.Workflow.Core.Model;
using System.Collections.ObjectModel;

namespace DevelApp.Workflow.Messages
{
    class ListAllWorkflowsMessage
    {
    }
    public class ListAllWorkflowsSucceededMessage
    {
        public ListAllWorkflowsSucceededMessage(ReadOnlyCollection<(KeyString, ReadOnlyCollection<SemanticVersionNumber>)> workflows)
        {
            Workflows = workflows;
        }

        public ReadOnlyCollection<(KeyString, ReadOnlyCollection<SemanticVersionNumber>)> Workflows { get; }
    }
}
