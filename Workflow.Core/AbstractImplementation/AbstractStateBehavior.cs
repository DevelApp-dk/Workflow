using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Core.AbstractImplementation
{
    /// <summary>
    /// Placement of utility functions for all StateBehaviors
    /// </summary>
    public abstract class AbstractStateBehavior
    {
        /// <summary>
        /// SagaStep is gathering data for the SagaStep
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        public abstract bool Initiate(ISagaStep sagaStep);

        /// <summary>
        /// Evaluates the gathered data and decides on an outcome 
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        public abstract bool Evaluate(ISagaStep sagaStep);
    }
}
