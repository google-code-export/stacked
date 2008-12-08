using System;
using System.Configuration;

namespace Utilities
{
    public enum RegisteringType { OpenID, Native };

    public static class Settings
    {
        public static bool AllowOpenID
        {
            get { return ConfigurationManager.AppSettings["allowOpenID"] == "true"; }
        }

        public static bool AllowNativeRegistering
        {
            get { return ConfigurationManager.AppSettings["allowNativeLogin"] == "true"; }
        }

        public static int CredsNeededToDeleteQuestion
        {
            get { return int.Parse(ConfigurationManager.AppSettings["CredsNeededToDeleteQuestion"]); }
        }

        public static int CredsNeededToEditQuestion
        {
            get { return int.Parse(ConfigurationManager.AppSettings["CredsNeededToEditQuestion"]); }
        }

        public static int CredsNeededToDeleteAnswer
        {
            get { return int.Parse(ConfigurationManager.AppSettings["CredsNeededToDeleteAnswer"]); }
        }

        public static int CredsNeededToEditAnswer
        {
            get { return int.Parse(ConfigurationManager.AppSettings["CredsNeededToEditAnswer"]); }
        }

        public static string GMapsAPIKey
        {
            get { return ConfigurationManager.AppSettings["GMapsAPIKey"]; }
        }

        public static bool GiveInternetExploderWarning
        {
            get { return ConfigurationManager.AppSettings["GiveInternetExploderWarning"] == "true"; }
        }

        public static TimeSpan SpanBeforeOrderingAnswersByVotes
        {
            get
            {
                int days = int.Parse(ConfigurationManager.AppSettings["SpanBeforeOrderingAnswersByVotes"]);
                if (days == 0)
                    return new TimeSpan();
                else if (days == -1)
                    return new TimeSpan(10000, 0, 0, 0);
                else
                    return new TimeSpan(days, 0, 0, 0);
            }
        }

        public static RegisteringType DefaultRegistering
        {
            get
            {
                // Doing some intelligence here in case admin of site messes up with config parts...!
                if (AllowOpenID && !AllowNativeRegistering)
                    return RegisteringType.OpenID;
                else if (!AllowOpenID && AllowNativeRegistering)
                    return RegisteringType.Native;
                else
                    return ConfigurationManager.AppSettings["defaultRegistering"] == "OpenID" ? 
                        RegisteringType.OpenID : 
                        RegisteringType.Native; }
        }
    }
}