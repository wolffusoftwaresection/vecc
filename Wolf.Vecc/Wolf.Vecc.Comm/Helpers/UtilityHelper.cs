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

        /// <summary>
        /// 初始化密码
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <param name="guidSalt">盐guid</param>
        /// <returns>加密过的密码串</returns>
        public static string CreateHashCodePW(string password, out string guidSalt)
        {
            guidSalt = Guid.NewGuid().ToString();
            byte[] passwordAndSaltBytes = Encoding.UTF8.GetBytes(password + guidSalt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="guidSalt"></param>
        /// <returns></returns>
        public static string CreateHashCodePW(string password, string guidSalt)
        {
            byte[] passwordAndSaltBytes = Encoding.UTF8.GetBytes(password + guidSalt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
