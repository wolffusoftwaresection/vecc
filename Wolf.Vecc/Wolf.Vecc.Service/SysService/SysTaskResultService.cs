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
    public class SysTaskResultService : ISysTaskResultService
    {
        private readonly IDbServiceReposity _dbServiceReposity;
        public SysTaskResultService(IDbServiceReposity dbServiceReposity)
        {
            _dbServiceReposity = dbServiceReposity;
        }

        public SysTaskResult GetTaskResultByTaskId(string guid)
        {
            return _dbServiceReposity.FirstOrDefault<SysTaskResult>(d => d.TaskID.ToString() == guid);
        }

        public bool UpdateTaskResultTaskId(string testPerson, string placeTest, string routeDescription, string testDate, string testTime, string taskID)
        {
            var result = _dbServiceReposity.FirstOrDefault<SysTaskResult>(d => d.TaskID.ToString() == taskID);
            result.TestPerson = testPerson;
            result.PlaceTest = placeTest;
            result.RouteDescription = routeDescription;
            result.TestDate = testDate;
            result.TestTime = testTime;

            return _dbServiceReposity.Update(result) > 0;
        }
    }
}
