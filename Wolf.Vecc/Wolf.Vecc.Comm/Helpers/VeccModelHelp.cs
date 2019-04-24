using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Comm.Helpers
{
    public static class VeccModelHelp
    {
        public static string GetUserType(int userType)
        {
            string _usertype = "";
            switch (userType)
            {
                case 1:
                    _usertype = "工程师";
                    break;
                case 2:
                    _usertype = "企业用户";
                    break;
                default:
                    _usertype = "检测机构";
                    break;
            }
            return _usertype;
        }

        public static string GetAccountStatus(int accountStatus)
        {
            var str = "";
            switch (accountStatus)
            {
                case 1:
                    str = "待审批";
                    break;
                case 2:
                    str = "未通过";
                    break;
                default:
                    str = "审批通过";
                    break;
            }
            return str;
        }
    }
}
