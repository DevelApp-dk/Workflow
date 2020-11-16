using DevelApp.Workflow.Model;

namespace DevelApp.Workflow.Interfaces
{
    public interface IDataOwnerCRUDMessage
    {
        CRUDMessageType CRUDMessageType { get; }
    }
}
