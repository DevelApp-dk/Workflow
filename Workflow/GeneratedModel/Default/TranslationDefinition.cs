using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ardalis.SmartEnum;
using System.Collections.Generic;
using System.Net.Mail;

namespace Default
{
    /// <summary>
    /// TranslationDefinition: The definition of a translation
    /// </summary>
    public partial class TranslationDefinition
    {
        /// <summary>
        /// Key: Lookup key for translation
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }


        /// <summary>
        /// Translation: Returns the translation
        /// </summary>
        [JsonProperty("translation")]
        public string Translation { get; set; }


    }
}
