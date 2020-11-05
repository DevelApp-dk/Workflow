using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Model
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
}
