using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevelApp.Workflow.Utilities
{
    /// <summary>
    /// Loads the configuration and returns an akka Config object
    /// </summary>
    public static class ConfigurationLoader
    {
        /// <summary>
        /// Loads file from disc
        /// </summary>
        /// <param name="fileNameAndPath"></param>
        /// <returns></returns>
        public static Config LoadFromDisc(string fileNameAndPath)
        {
            if (File.Exists(fileNameAndPath))
            {
                string config = File.ReadAllText(fileNameAndPath);
                return ConfigurationFactory.ParseString(config);
            }

            return Config.Empty;
        }
    }
}
