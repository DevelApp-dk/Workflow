using DevelApp.Workflow.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Interfaces
{
    public interface IDataOwnerCRUDMessage
    {
        CRUDMessageType CRUDMessageType { get; }
    }
}
