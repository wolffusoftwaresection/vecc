using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.IService.ISysService
{
    public interface ISysTaskService
    {
        int Insert(SysTask sysUser);
    }
}
