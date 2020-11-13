using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Model
{
    public class ModuleDefinition
    {
        /// <summary>
        /// Returns the Module unique name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns the version of the Module
        /// </summary>
        public long Version { get; }

        /// <summary>
        /// Returns the embedded workflow definitions
        /// </summary>
        public List<WorkflowDefinition> WorkflowDefinitions { get; }
    }
}
