using DevelApp.Workflow.Model;

namespace DevelApp.Workflow.Interfaces
{
    public interface IWorkflowCRUDMessage
    {
        CRUDMessageType CRUDMessageType { get; }
    }
}
