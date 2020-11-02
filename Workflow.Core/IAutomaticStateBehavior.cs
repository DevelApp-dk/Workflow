using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Core
{
    /// <summary>
    /// Automatic progress the SagaStep throught the SagaStepState from Initiate to Evaluate
    /// </summary>
    public interface IAutomaticStateBehavior:IStateBehavior
    {
    }
}
