using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.IService.ISysService;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.Service.SysService
{
    public class AccountService : IAccountService
    {
        private readonly ISysUserService _userService;
        private readonly ISysRoleService _roleService;
        public AccountService(ISysUserService userService, ISysRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public List<int> GetRoleIdByUserId(int uId)
        {
            return _userService.GetRoleIdByUserId(uId);
        }

        public List<string> GetRoleNamesByRoleIds(List<int> roleId)
        {
            return _roleService.GetRoleNamesByRoleIds(roleId);
        }

        public List<string> GetRoleNamesByRoleIds(int roleId)
        {
            return _roleService.GetRoleNameByRoleId(roleId);
        }

        /// <summary>
        /// 是否存在用户名
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>返回false为不存在</returns>
        public SysUsers GetUserByUserName(string userName)
        {
            return _userService.GetUserByUserName(userName);
        }

        public List<SysUsers> GetUsers()
        {
            return _userService.All();
        }
    }
}
