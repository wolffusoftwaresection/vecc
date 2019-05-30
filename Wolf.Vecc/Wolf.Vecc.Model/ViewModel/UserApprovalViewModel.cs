using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model.ViewModel
{
    public class UserApprovalViewModel: PageBaseModel
    {
        public string UserName { get; set; }

        public int State { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class DataApprovalViewModel : PageBaseModel
    {
        public int DataState { get; set; }

        public string FileName { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class FindingsAuditViewModel : PageBaseModel
    {
        public int DataState { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
