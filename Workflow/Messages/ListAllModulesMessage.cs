using DevelApp.Utility.Model;
using System.Collections.ObjectModel;

namespace DevelApp.Workflow.Messages
{
    public class ListAllModulesMessage
    {
    }

    public class ListAllModulesSucceededMessage
    {
        public ListAllModulesSucceededMessage(ReadOnlyCollection<(KeyString, ReadOnlyCollection<SemanticVersionNumber>)> modules)
        {
            Modules = modules;
        }

        public ReadOnlyCollection<(KeyString, ReadOnlyCollection<SemanticVersionNumber>)> Modules { get; }
    }
}
