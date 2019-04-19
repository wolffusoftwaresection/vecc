﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Comm.EnumConfigs
{
    public enum EnumGlobalConfig
    {
        [Description("Session超时")]
        SessionTimeOut,
        [Description("vecc管理员初始密码设置")]
        AdminInitialPassword
    }
}