using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.IService.ISysService
{
    public interface ISysApprovalUserService
    {
        int Insert(SysApprovaUser sysApprovaUser);

        int BatchInsert(List<SysApprovaUser> sysApprovaUsers);

        SysApprovaUser GetSysApprovaUserByUserId(int userId);
    }
}
