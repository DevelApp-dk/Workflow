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
            if (!fileNameAndPathUri.IsFile)
            {
                throw new WorkflowStartupException("The supplied Uri is not a file Uri");
            }

            if (File.Exists(fileNameAndPathUri.LocalPath))
            {
                try
                {
                    string config = File.ReadAllText(fileNameAndPathUri.LocalPath);
                    return ConfigurationFactory.ParseString(config);
                }
                catch (Exception ex)
                {
                    throw new WorkflowStartupException($"Error occured when trying to read configuration file [{fileNameAndPathUri.LocalPath}]", ex);
                }
            }

            return Config.Empty;
        }
    }
}
