using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Comm.EnumConfigs;

namespace Wolf.Vecc.Comm.Helpers
{
    public static class GlobalConfigHelper
    {
        public static int GetSessionTimeOut()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings[EnumGlobalConfig.SessionTimeOut.ToString()].ToString().Trim());
        }

        public static string GetAdminInitialPassword()
        {
            return ConfigurationManager.AppSettings[EnumGlobalConfig.AdminInitialPassword.ToString()].Trim();
        }

        public static string GetFileUrl()
        {
            return ConfigurationManager.AppSettings[EnumGlobalConfig.FileUrl.ToString()].Trim();
        }
    }
}
