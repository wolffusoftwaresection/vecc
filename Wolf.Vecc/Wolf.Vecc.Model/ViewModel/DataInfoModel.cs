using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model.ViewModel
{
    public class DataInfoModel
    {
        public string UserName {get;set;}

        public string UserPhone { get; set; }

        public string UserEmail { get; set; }

        public string UserType { get; set; }

        public string AccountStatus { get; set; }

        public DateTime? AccountData { get; set; }

        public DateTime? UpLoadDate { get; set; }

        public string EnterpriseName { get; set; }
        public string Reason { get; set; }
    }
}
