﻿using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Core
{
    public interface ISagaStepBehaviorFactory
    {
        /// <summary>
        /// Returns an instance of the behavior
        /// </summary>
        /// <param name="behaviorName"></param>
        /// <returns></returns>
        ISagaStepBehavior GetStateBehavior(string behaviorName, int version, JsonValue behaviorConfiguration);
    }
}
