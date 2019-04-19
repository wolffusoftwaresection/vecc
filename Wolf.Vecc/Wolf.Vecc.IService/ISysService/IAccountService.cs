using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.IService.ISysService
{
    public interface IAccountService
    {
        /// <summary>
        /// 通过用户编号获取用户角色
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        List<int> GetRoleIdByUserId(int uId);

        /// <summary>
        /// 根据角色编号列表获取角色名称列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<string> GetRoleNamesByRoleIds(List<int> roleId);

        List<string> GetRoleNamesByRoleIds(int roleId);

        List<SysUsers> GetUsers();

        /// <summary>
        /// 是否存在用户名
        /// </summary>
        /// <returns></returns>
        SysUsers GetUserByUserName(string userName);
    }
}
