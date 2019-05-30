using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model.ResultModel
{
    public class UserFindingsAuditModel
    {
        public int Id { get; set; }
        public DateTime? UploadDate { get; set; }
        public string DataName { get; set; }
        public int DataStatus { get; set; }
        public string DataUrl { get; set; }
        public string ApprovalDate { get; set; }
        public string ApprovalRemark { get; set; }
    }
}
