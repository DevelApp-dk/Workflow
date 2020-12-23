using DevelApp.Workflow.Model;

namespace DevelApp.Workflow.Interfaces
{
    public interface IModuleCRUDMessage
    {
        CRUDMessageType CRUDMessageType { get; }
    }
}
