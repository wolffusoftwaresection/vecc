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

        public int BatchDelete(string ids)
        {
            string sql = @"delete FROM sys_approva_data WHERE date_id in ({0})";
            return _dbServiceReposity.ExecuteSqlCommand(string.Format(sql, ids), new System.Data.SqlClient.SqlParameter[0]);
        }

        public int BatchInsert(List<SysApprovaData> sysApprovaDatas)
        {
            return _dbServiceReposity.AddRange(sysApprovaDatas);
        }

        public int Insert(SysApprovaData sysApprovaData)
        {
            return _dbServiceReposity.Add(sysApprovaData);
        }
    }
}
