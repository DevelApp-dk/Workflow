using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core.AbstractImplementation
{
    /// <summary>
    /// Convenience implementation of IAutomaticStateBehavior
    /// </summary>
    public abstract class AbstractAutomaticStateBehavior : AbstractStateBehavior, IAutomaticStateBehavior
    {
        public StateBehaviorType BehaviorType
        {
            get
            {
                return StateBehaviorType.Automatic;
            }
        }
    }
}
