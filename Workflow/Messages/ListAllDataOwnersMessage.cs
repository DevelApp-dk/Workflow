using DevelApp.Workflow.Core.Model;
using System.Collections.ObjectModel;

namespace DevelApp.Workflow.Messages
{
    public class ListAllDataOwnersMessage
    {
    }

    public class ListAllDataOwnersSucceededMessage
    {
        public ListAllDataOwnersSucceededMessage(ReadOnlyCollection<(KeyString, ReadOnlyCollection<SemanticVersionNumber>)> dataOwners)
        {
            DataOwners = dataOwners;
        }

        public ReadOnlyCollection<(KeyString, ReadOnlyCollection<SemanticVersionNumber>)> DataOwners { get; }
    }
}
