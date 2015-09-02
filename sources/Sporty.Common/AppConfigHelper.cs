using System;
using System.Configuration;

namespace Sporty.Common
{
    public static class AppConfigHelper
    {
        public static string GetWebConfigValue(string key)
        {
            string retVal = String.Empty;
            try
            {
                retVal = @ConfigurationManager.AppSettings[key];
            }
            catch (Exception exc)
            {
                //LogManager.LogExceptionThrow(exc);
            }

            if (retVal.Length < 1)
            {
                //LogManager.LogExceptionThrow(new ConfigurationErrorsException(key + " is not set in web.config!"));
            }
            return retVal;
        }

        public static ConfigurationSection GetWebConfigSection(string key)
        {
            ConfigurationSection retVal = null;
            try
            {
                retVal = (ConfigurationSection) @ConfigurationManager.GetSection(key);
            }
            catch (Exception exc)
            {
                //LogManager.LogExceptionThrow(exc);
            }
            return retVal;
        }

        public static void SetConfigValue(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}