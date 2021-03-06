﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.IService.ISysService
{
    public interface ISysUserService
    {
        int Insert(SysUsers sysUser);

        int Update(SysUsers sysUser);

        List<SysUsers> All();

        SysUsers GetUserByUserName(string userName);

        List<int> GetRoleIdByUserId(int uId);
    }
}
