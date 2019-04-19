using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Data.DataService;
using Wolf.Vecc.IService.ISysService;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.Service.SysService
{
    public class SysRoleService : ISysRoleService
    {
        private readonly IDbServiceReposity _dbServiceReposity;
        public SysRoleService(IDbServiceReposity dbServiceReposity)
        {
            _dbServiceReposity = dbServiceReposity;
        }

        public List<SysRole> All()
        {
            return _dbServiceReposity.All<SysRole>().Where(d => d.IsDel == 0).ToList();
        }

        public List<string> GetRoleNameByRoleId(int roleId)
        {
            return _dbServiceReposity.Where<SysRole>(d => roleId == d.Id && d.IsDel == 0).Select(d => d.RoleName).ToList();
        }

        public List<string> GetRoleNamesByRoleIds(List<int> roleId)
        {
            //后期使用缓存中的角色表
            return _dbServiceReposity.Where<SysRole>(d => roleId.Contains(d.Id) && d.IsDel == 0).Select(d => d.RoleName).ToList();
        }
    }
}
