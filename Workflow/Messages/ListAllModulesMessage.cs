using DevelApp.Workflow.Core.Model;
using System.Collections.ObjectModel;

namespace DevelApp.Workflow.Messages
{
    public class ListAllModulesMessage
    {
    }

    public class ListAllModulesSucceededMessage
    {
        public ListAllModulesSucceededMessage(ReadOnlyCollection<(KeyString, ReadOnlyCollection<VersionNumber>)> modules)
        {
            Modules = modules;
        }

        public ReadOnlyCollection<(KeyString, ReadOnlyCollection<VersionNumber>)> Modules { get; }
    }
}
