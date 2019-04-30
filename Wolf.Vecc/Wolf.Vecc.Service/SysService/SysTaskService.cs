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
    public class SysTaskService: ISysTaskService
    {
        private readonly IDbServiceReposity _dbServiceReposity;
        public SysTaskService(IDbServiceReposity dbServiceReposity)
        {
            _dbServiceReposity = dbServiceReposity;
        }

        public SysTask GetSysTaskByTaskId(string taskId)
        {
            return _dbServiceReposity.FirstOrDefault<SysTask>(d => d.TaskId.ToString() == taskId);
        }

        public int Insert(SysTask sysTask)
        {
            return _dbServiceReposity.Add(sysTask);
        }

        public bool UpdateSysTaskStatus(Guid taskID, int taskStatus)
        {
            var result = _dbServiceReposity.FirstOrDefault<SysTask>(d => d.TaskId.ToString() == taskID.ToString());
            result.TaskStatus = taskStatus;
            return _dbServiceReposity.Update(result) > 0;
        }
    }
}
