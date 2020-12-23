using DevelApp.Utility.Model;
using System.Collections.ObjectModel;

namespace DevelApp.Workflow.Messages
{
    public class ListAllWorkflowsMessage
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
