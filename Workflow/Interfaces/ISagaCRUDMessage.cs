using DevelApp.Workflow.Model;

namespace DevelApp.Workflow.Interfaces
{
    public interface ISagaCRUDMessage
    {
        CRUDMessageType CRUDMessageType { get; }
    }
}
