using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ardalis.SmartEnum;
using System.Collections.Generic;
using System.Net.Mail;

namespace Default
{
    /// <summary>
    /// ConfigurationDefinition: The definition of a configuration
    /// </summary>
    public partial class ConfigurationDefinition
    {
        /// <summary>
        /// Key: Lookup key for configuration
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }


        /// <summary>
        /// Configuration: Returns the configuration
        /// </summary>
        [JsonProperty("configuration")]
        public string Configuration { get; set; }


    }
}
