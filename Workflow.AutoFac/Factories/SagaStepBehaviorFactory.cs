using DevelApp.RuntimePluggableClassFactory;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.Model;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelApp.Workflow.Factories
{
    //TODO Move to Worflow Project if standard library is given up as it requires core 3.1 to work

    /// <summary>
    /// Holds the factory for 
    /// </summary>
    public class SagaStepBehaviorFactory : ISagaStepBehaviorFactory
    {
        public SagaStepBehaviorFactory()
        {
            PluginClassFactory<ISagaStepBehavior> pluginSagaStepBehaviorFactory = new PluginClassFactory<ISagaStepBehavior>(10);
        }

        public ISagaStepBehavior GetStateBehavior(KeyString behaviorName, VersionNumber version, JsonValue behaviorConfiguration)
        {
            throw new NotImplementedException();
        }
    }
}
