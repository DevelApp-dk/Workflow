using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ardalis.SmartEnum;
using System.Collections.Generic;
using System.Net.Mail;

namespace Default
{
    /// <summary>
    /// DataOwnerDefinition: The easy definition of the schema of a dataOwner
    /// </summary>
    public partial class DataOwnerDefinition
    {
        /// <summary>
        /// Name: Returns the DataOwner unique name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }


        /// <summary>
        /// Version: Returns the version of the DataOwner
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }


        /// <summary>
        /// ModuleDefinitions: Returns the embedded module definitions
        /// </summary>
        [JsonProperty("moduleDefinitions")]
        public List<ModuleDefinition> ModuleDefinitions { get; set; } = new List<ModuleDefinition>();


    }
}
