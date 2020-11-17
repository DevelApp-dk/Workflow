using DevelApp.Workflow.Core.Model;
using System.Collections.ObjectModel;

namespace DevelApp.Workflow.Messages
{
    class ListAllWorkflowsMessage
    {
    }
    public class ListAllWorkflowsSucceededMessage
    {
        public ListAllWorkflowsSucceededMessage(ReadOnlyCollection<(KeyString, ReadOnlyCollection<VersionNumber>)> workflows)
        {
            Workflows = workflows;
        }

        public ReadOnlyCollection<(KeyString, ReadOnlyCollection<VersionNumber>)> Workflows { get; }
    }
}
