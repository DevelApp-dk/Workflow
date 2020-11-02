using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Core
{
    /// <summary>
    /// Defines the topmost interface on StateBehavior which is the executable part of a Workflow. This should not be implemented directly
    /// </summary>
    public interface IStateBehavior
    {
        /// <summary>
        /// Defines the specific state behavior used for casting
        /// </summary>
        StateBehaviorType BehaviorType { get; }

        /// <summary>
        /// SagaStep is gathering data for the SagaStep
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        bool Initiate(ISagaStep sagaStep);

        /// <summary>
        /// Evaluates the gathered data and decides on an outcome 
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        bool Evaluate(ISagaStep sagaStep);
    }
}
