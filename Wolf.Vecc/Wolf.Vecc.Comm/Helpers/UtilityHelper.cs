using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Comm.Helpers
{
    public static class UtilityHelper
    {
        /// <summary>
        /// 获取ip地址 本地获取不到
        /// </summary>
        /// <returns></returns>
        public static string GetUserIp()
        {
            string realRemoteIP = "";
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                realRemoteIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];
            }
            if (string.IsNullOrEmpty(realRemoteIP))
            {
                realRemoteIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(realRemoteIP))
            {
                realRemoteIP = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            return realRemoteIP;
        }
    }
}
