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
    public class SysPemsTaskService : ISysPemsTaskService
    {
        private readonly IDbServiceReposity _dbServiceReposity;
        public SysPemsTaskService(IDbServiceReposity dbServiceReposity)
        {
            _dbServiceReposity = dbServiceReposity;
        }

        public SysPemsTask GetSysPemsTaskByTaskId(string taskId)
        {
            return _dbServiceReposity.FirstOrDefault<SysPemsTask>(d => d.TaskId.ToString() == taskId);
        }

        public int Insert(SysPemsTask sysPemsTask)
        {
            return _dbServiceReposity.Add(sysPemsTask);
        }
    }
}
