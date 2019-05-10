using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model.ResultModel
{
    public class TaskResultModel
    {
        public string TaskId{get;set;}

        public string VehicleType { get;set;}

        public string TestDate { get; set; }

        public string TestTime { get; set; }

        public string TestPerson { get; set; }

        public string PlaceTest { get; set; }

        public string RouteDescription { get; set; }
    }
}
