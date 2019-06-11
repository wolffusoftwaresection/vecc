using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Wolf.Vecc.Comm.Helpers;
using Wolf.Vecc.IService.IReadFileService;
using Wolf.Vecc.Model.ResultModel;

namespace Wolf.Vecc.Service.ReadFileService
{
    public class ReadRstService : IReadRstService
    {
        public Dictionary<string, string> ReadTestInfo(string Result_Root_Url, string taskId)
        {
            string line = "";
            string url = string.Format(Result_Root_Url, taskId);
            StreamReader file = new StreamReader(url, Encoding.Default);
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            while ((line = file.ReadLine()) != null)
            {
                if (line.Trim() != "")
                {
                    string[] arraynamevalue = line.Split('=');
                    if (arraynamevalue.Length == 2)
                    {
                        keyValuePairs.Add(arraynamevalue[0], arraynamevalue[1]);
                    }
                }
            }
            return keyValuePairs == null ? null : keyValuePairs;
        }
        //public TestInfoModel ReadTestInfo(string taskId)
        //{
        //    string line = "";
        //    string url = string.Format(Result_Root_Url, taskId);

        //    StreamReader file = new StreamReader(url, System.Text.Encoding.Default);
        //    while ((line = file.ReadLine()) != null)
        //    {
        //        if (line.Trim() != "")
        //        {
        //            string[] arraynamevalue = line.Split('=');
        //            if (arraynamevalue.Length == 2)
        //            {
        //                name = arraynamevalue[0];
        //                value = arraynamevalue[1];
        //            }
        //        }
        //    }
        //    return null;
        //}
    }
}
