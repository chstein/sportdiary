using System.Collections.Generic;
namespace Sporty.Common
{
    public static class Constants
    {
        // email constants
        public const string EmailEnabled = "emailEnabled";
        public const string SmtpServer = "smtpserver";
        public const string SmtpServerPort = "smtpserverport";
        public const string SendUsername = "sendusername";
        public const string SendPassword = "sendpassword";
        public const string DisplayName = "displayname";
        public const string SmtpuseSSL = "smtpusessl";

        public const string UseDefaultFolder = "useDefaultFolder";
        public const string DefaultFolder = "defaultFolder";

        //#region LogTypes
        //public enum LogTypes
        //{
        //    Info = 0
        //}

        //public const string sloggingConfiguration = "loggingConfiguration";
        //public const string sName = "name";
        //public const string sFileName = "fileName";
        //#endregion

        //public const string EmailRegex = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*";

        public const string RootUploadFolder = "rootuploadfolder";
        public static string ConnectionString = "SportyEntities";
        public static string CreateNewMetricJobMailTemplate = "CreateNewMetricJobMailTemplate";

        public static List<WeatherCondition> AllWeatherConditions = new List<WeatherCondition>{
           new WeatherCondition { SelectValue = "", ImageFilepath = "transparent.png", Text="Kein Wetter"},
        new WeatherCondition { SelectValue = "cloudy", ImageFilepath = "cloudy.png", Text="Bewölkt"},
        new WeatherCondition { SelectValue = "drizzle", ImageFilepath = "drizzle.png", Text="Regen"},
        new WeatherCondition { SelectValue = "haze", ImageFilepath = "haze.png", Text="Sturm"},
        new WeatherCondition { SelectValue = "mostlycloudy", ImageFilepath = "mostlycloudy.png", Text="Leicht Bewölkt"},
        new WeatherCondition { SelectValue = "slightdrizzle", ImageFilepath = "slightdrizzle.png", Text="Leichter Regen"},
        new WeatherCondition { SelectValue = "snow", ImageFilepath = "snow.png", Text="Schnee"},
        new WeatherCondition { SelectValue = "sunny", ImageFilepath = "sunny.png", Text="Sonne"},
        new WeatherCondition { SelectValue = "thunderstorms", ImageFilepath =  "thunderstorms.png", Text="Unwetter"}
        };
        //{1, "storm"}, 
        //{2, "storm"}, 
        //{3, "storm"}, 
        //{4, "lightning"}, 
        //{5, "lightning"}, 
        //{6, "snow"}, 
        //{7, "hail"}, 
        //{8, "hail"}, 
        //{9, "drizzle"}, 
        //{10, "drizzle"}, 
        //{11, "rain"}, 
        //{12, "rain"}, 
        //{13, "rain"}, 
        //{14, "snow"}, 
        //{15, "snow"}, 
        //{16, "snow"}, 
        //{17, "snow"}, 
        //{18, "hail"}, 
        //{19, "hail"}, 
        //{20, "fog"}, 
        //{21, "fog"}, 
        //{22, "fog"}, 
        //{23, "fog"}, 
        //{24, "wind"}, 
        //{25, "wind"}, 
        //{26, "snowflake"}, 
        //{27, "cloud"}, 
        //{28, "cloud_moon"}, 
        //{29, "cloud_sun"}, 
        //{30, "cloud_moon"}, 
        //{31, "cloud_sun"}, 
        //{32, "moon"}, 
        //{33, "sun"}, 
        //{34, "moon"}, 
        //{35, "sun"}, 
        //{36, "hail"}, 
        //{37, "sun"}, 
        //{39, "lightning"}, 
        //{40, "lightning"}, 
        //{41, "lightning"}, 
        //{42, "rain"}, 
        //{43, "snowflake"}, 
        //{44, "snowflake"}, 
        //{45, "snowflake"}, 
        //{46, "cloud"}, 
        //{47, "rain"}, 
        //{48, "snow"}, 
        //{49, "lightning"}
        //"storm", "storm", "lightning", "lightning", "snow", "hail", "hail",
        //"drizzle", "drizzle", "rain", "rain", "rain", "snow", "snow", "snow", "snow",
        //"hail", "hail", "fog", "fog", "fog", "fog", "wind", "wind", "snowflake",
        //"cloud", "cloud_moon", "cloud_sun", "cloud_moon", "cloud_sun", "moon", "sun",
        //"moon", "sun", "hail", "sun", "lightning", "lightning", "lightning", "rain",
        //"snowflake", "snowflake", "snowflake", "cloud", "rain", "snow", "lightning"
        //    };

    }
}