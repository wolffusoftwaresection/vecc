using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Model.SysModel;
using Wolf.Vecc.Model.ViewModel;

namespace Wolf.Vecc.IService.ISysService
{
    public interface ISysUserService
    {
        int Insert(SysUsers sysUser);

        int Update(SysUsers sysUser);

        List<SysUsers> All();

        SysUsers GetUserByUserName(string userName);

        List<int> GetRoleIdByUserId(int uId);

        bool UserRepeat(string userName);

        List<SysUsers> GetUserList(UserApprovalViewModel userApprovalViewModel);

        SysUsers GetUserById(int Id);

        int GetApprovalNumber();

        int BatchDelete(string ids);

        int BatchUpdataUser(string sysUsers, int state);
    }
}
