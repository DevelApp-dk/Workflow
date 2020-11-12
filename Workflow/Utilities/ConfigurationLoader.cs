using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DevelApp.Workflow.Core.Exceptions;

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
        /// <param name="fileNameAndPathUri"></param>
        /// <returns></returns>
        public static Config LoadFromDisc(Uri fileNameAndPathUri)
        {
            if(!fileNameAndPathUri.IsFile)
            {
                throw new WorkflowStartupException("The supplied Uri is not a file Uri")
            }

            if (File.Exists(fileNameAndPath))
            {
                string config = File.ReadAllText(fileNameAndPath);
                return ConfigurationFactory.ParseString(config);
            }

            return Config.Empty;
        }
    }
}
