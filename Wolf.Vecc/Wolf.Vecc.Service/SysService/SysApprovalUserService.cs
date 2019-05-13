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
    public class SysApprovalUserService : ISysApprovalUserService
    {
        private readonly IDbServiceReposity _dbServiceReposity;
        public SysApprovalUserService(IDbServiceReposity dbServiceReposity)
        {
            _dbServiceReposity = dbServiceReposity;
        }

        public int Insert(SysApprovaUser sysApprovaUser)
        {
            return _dbServiceReposity.Add(sysApprovaUser);
        }
    }
}
