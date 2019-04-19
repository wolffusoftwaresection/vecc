using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.IService.ISysService
{
    public interface ISysRoleService
    {
        List<SysRole> All();

        List<string> GetRoleNamesByRoleIds(List<int> roleId);

        List<string> GetRoleNameByRoleId(int roleId);
    }
}
