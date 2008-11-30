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