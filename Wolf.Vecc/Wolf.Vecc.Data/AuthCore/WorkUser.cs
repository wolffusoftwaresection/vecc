using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.Data.AuthCore
{
    public class WorkUser
    {
        /// <summary>
        /// 当前登陆用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 当前登陆用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 当前登陆用户的账号状态(审核状态)1待审批 ，2未通过，3审批通过当为3时为审批通过可登陆
        /// </summary>
        public int AaccountStatus { get; set; }

        /// <summary>
        /// 当前登陆用户的角色编号
        /// </summary>
        public int RoleId { get; set; }
    }
}
