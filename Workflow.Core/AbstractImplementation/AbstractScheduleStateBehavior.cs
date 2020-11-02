using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Core.AbstractImplementation
{
    /// <summary>
    /// Convenience implementation of IScheduleStateBehavior
    /// </summary>
    public abstract class AbstractScheduleStateBehavior : AbstractStateBehavior, IScheduleStateBehavior
    {
        public StateBehaviorType BehaviorType
        {
            get
            {
                return StateBehaviorType.Schedule;
            }
        }

        /// <summary>
        /// SagaStep is waiting for a sheduled timer before progressing. Will go out of memory just after scheduling as the schedule might be in long time
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        public abstract bool BeforeWaitForState(ISagaStep sagaStep);
    }
}
