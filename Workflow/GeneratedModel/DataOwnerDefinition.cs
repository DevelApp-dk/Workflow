using DevelApp.Workflow.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Model
{
    public class DataOwnerDefinition
    {
        /// <summary>
        /// Returns the DataOwner unique name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns the version of the DataOwner
        /// </summary>
        public long Version { get; }

        /// <summary>
        /// Returns the embedded module definitions
        /// </summary>
        public List<ModuleDefinition> ModuleDefinitions { get; }
    }
}
