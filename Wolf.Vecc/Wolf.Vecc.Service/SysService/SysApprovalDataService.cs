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
    public class SysApprovalDataService: ISysApprovalDataService
    {
        private readonly IDbServiceReposity _dbServiceReposity;
        public SysApprovalDataService(IDbServiceReposity dbServiceReposity)
        {
            _dbServiceReposity = dbServiceReposity;
        }

        public int Insert(SysApprovaData sysApprovaData)
        {
            return _dbServiceReposity.Add(sysApprovaData);
        }
    }
}
