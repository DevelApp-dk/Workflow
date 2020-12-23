using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ardalis.SmartEnum;
using System.Collections.Generic;
using System.Net.Mail;

namespace Default
{
    /// <summary>
    /// LanguageDefinition: The definition of a language
    /// </summary>
    public partial class LanguageDefinition
    {
        /// <summary>
        /// Key: Lookup key for language
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }


        /// <summary>
        /// Language: Returns the language
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }


    }
}
