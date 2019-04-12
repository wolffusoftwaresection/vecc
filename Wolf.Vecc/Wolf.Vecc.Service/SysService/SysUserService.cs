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
    public class SysUserService : ISysUserService
    {
        private readonly IDbServiceReposity _dbServiceReposity;
        public SysUserService(IDbServiceReposity dbServiceReposity)
        {
            _dbServiceReposity = dbServiceReposity;
        }

        public List<SysUser> All()
        {
            return _dbServiceReposity.All<SysUser>().Where(d => d.IsDel == 0).ToList();
        }

        public int Insert(SysUser sysUser)
        {
            return _dbServiceReposity.Add(sysUser);
        }

        public int Update(SysUser sysUser)
        {
            return _dbServiceReposity.Update(sysUser);
        }
    }
}
