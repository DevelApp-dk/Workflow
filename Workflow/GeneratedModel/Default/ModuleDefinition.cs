using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ardalis.SmartEnum;
using System.Collections.Generic;
using System.Net.Mail;

namespace Default
{
    /// <summary>
    /// ModuleDefinition: The easy definition of the schema of a module
    /// </summary>
    public partial class ModuleDefinition
    {
        /// <summary>
        /// Name: Returns the Module unique name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }


        /// <summary>
        /// Version: Returns the version of the Module
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }


        /// <summary>
        /// WorkflowDefinitions: Returns the embedded workflow definitions
        /// </summary>
        [JsonProperty("workflowDefinitions")]
        public List<WorkflowDefinition> WorkflowDefinitions { get; set; } = new List<WorkflowDefinition>();


    }
}
