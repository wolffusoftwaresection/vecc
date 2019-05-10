using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Model.ResultModel;

namespace Wolf.Vecc.IService.IReadFileService
{
    public interface IReadRstService
    {
        //TestInfoModel ReadTestInfo(string taskId);

        Dictionary<string, string> ReadTestInfo(string Result_Root_Url, string taskId);
    }
}
