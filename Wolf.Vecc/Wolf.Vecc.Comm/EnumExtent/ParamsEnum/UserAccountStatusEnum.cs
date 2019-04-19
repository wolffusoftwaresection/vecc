using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Comm.EnumExtent.ParamsEnum
{
    /// <summary>
    /// [Description("用户审核状态")]
    /// </summary>
    public enum UserAccountStatusEnum
    {
        /// <summary>
        /// 待审批
        /// </summary>
        [Description("待审批")]
        PENDING = 1,
        /// <summary>
        /// 未通过
        /// </summary>
        [Description("未通过")]
        FAIL = 2,
        /// <summary>
        /// 已通过
        /// </summary>
        [Description("已通过")]
        PASSED = 3
    }
}
