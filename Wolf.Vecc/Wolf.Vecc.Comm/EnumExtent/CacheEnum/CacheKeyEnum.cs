using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Comm.EnumExtent.CacheEnum
{
    public enum CacheKeyEnum
    {
        [Description("数据审核状态")]
        DataApproveKey,
        [Description("个人审核状态")]
        UserApproveKey,
        [Description("个人审核数据")]
        UserKey,
        [Description("数据审核数据")]
        DataKey
    }
}
