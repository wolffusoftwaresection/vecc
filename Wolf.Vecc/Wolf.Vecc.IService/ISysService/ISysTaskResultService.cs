using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.IService.ISysService
{
    public interface ISysTaskResultService
    {
        SysTaskResult GetTaskResultByTaskId(string guid);

        bool UpdateTaskResultTaskId(string testPerson, string placeTest, string routeDescription, string testDate, string testTime, Guid taskID);
    }
}
