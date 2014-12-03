using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Sporty.Infrastructure
{
    public static class ProviderUtils
    {
        public static object GetConfigValue(NameValueCollection config, string configKey, object defaultValue)
        {
            object configValue;

            try
            {
                configValue = config[configKey];
                configValue = string.IsNullOrEmpty(configValue.ToString()) ? defaultValue : configValue;
            }
            catch
            {
                configValue = defaultValue;
            }

            return configValue;
        }


    }
}
