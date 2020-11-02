using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Workflow.Core.AbstractImplementation
{
    /// <summary>
    /// Convenience implementation of IUserInteractionStateBehavior
    /// </summary>
    public abstract class AbstractUserInteractionStateBehavior : AbstractStateBehavior, IUserInteractionStateBehavior
    {
        public StateBehaviorType BehaviorType
        {
            get
            {
                return StateBehaviorType.UserInteraction;
            }
        }

        /// <summary>
        /// SagaStep is waiting for user interaction that is data. Will proces messages for intermediate data and save that on reception
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        public abstract bool BeforeWaitForUserInteraction(ISagaStep sagaStep);
    }
}
