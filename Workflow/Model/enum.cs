using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Model
{
    public enum Recipient
    {
        Unknown = 0,
        DataOwner = 1,
        Module = 2,
        Configuration = 3,
        JsonSchema = 4,
        Translation = 5,
        Workflow = 6,
        Saga = 7,
        User = 8
    }

    public enum CRUDMessageType
    {
        Undefined = 0,
        Create = 1,
        Update = 2,
        Delete = 3
    }
}
