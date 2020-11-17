using DevelApp.Workflow.Core.Model;
using System.Collections.ObjectModel;

namespace DevelApp.Workflow.Messages
{
    class ListAllSagasMessage
    {
    }
    public class ListAllSagasSucceededMessage
    {
        public ListAllSagasSucceededMessage(ReadOnlyCollection<(KeyString, ReadOnlyCollection<VersionNumber>)> sagas)
        {
            Sagas = sagas;
        }

        public ReadOnlyCollection<(KeyString, ReadOnlyCollection<VersionNumber>)> Sagas { get; }
    }
}
