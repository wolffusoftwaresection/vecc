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

        public List<SysUsers> All()
        {
            return _dbServiceReposity.All<SysUsers>().Where(d => d.IsDel == 0).ToList();
        }

        public List<int> GetRoleIdByUserId(int uId)
        {
            return _dbServiceReposity.Where<SysUsers>(d => d.IsDel == 0 && d.Id == uId).Select(d=>d.RoleId).ToList();
        }

        public SysUsers GetUserByUserName(string userName)
        {
            return _dbServiceReposity.FirstOrDefault<SysUsers>(d => d.UserName == userName.Trim() && d.IsDel == 0);
        }

        public int Insert(SysUsers sysUser)
        {
            return _dbServiceReposity.Add(sysUser);
        }

        public int Update(SysUsers sysUser)
        {
            return _dbServiceReposity.Update(sysUser);
        }
    }
}
