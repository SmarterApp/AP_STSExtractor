using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NLog;
using STSCommon.Extensions;
using STSCommon.Models;

namespace STSCommon.Utilities
{
    public class ApplicationSettings : Dictionary<string, string>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ApplicationSettings()
        {
            Add("ValidBankKeys", string.Empty);
        }

        public void Initialize()
        {
            var configurationReader = new AppSettingsReader();
            try
            {
                Keys.ToList().ForEach(x => this[x] = (string) configurationReader.GetValue(x, typeof(string)));
            }
            catch (Exception)
            {
                Logger.LogError(new ErrorReportItem
                    {
                        Location = "app.config",
                        Severity = LogLevel.Fatal
                    },
                    "One or more application configuration properties in app.config is missing or malformed. Refer to readme for full listing of required properties.");
                throw;
            }
        }
    }
}