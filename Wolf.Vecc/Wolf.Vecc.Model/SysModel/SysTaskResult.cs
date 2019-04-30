using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model.SysModel
{
    [Table("Sys_TaskResult")]
    public class SysTaskResult
    {
        [Column("TaskResultID")]
        [Key]
        public Guid TaskResultID { get; set; }

        public DateTime TaskFinishTime { get; set; }

        [DataType(DataType.Text)]
        public string PdfReportUrl { get; set; }

        public Guid TaskID { get; set; }

        [DataType(DataType.Text)]
        public string ErrorInfo { get; set; }

        [DataType(DataType.Text)]
        public string ResultDirUrl{ get; set; }

        [StringLength(20)]
        public string TestPerson { get; set; }

        [StringLength(128)]
        public string PlaceTest { get; set; }

        [DataType(DataType.Text)]
        public string RouteDescription { get; set; }

        [StringLength(50)]
        public string TestDate { get; set; }

        [StringLength(50)]

        public string TestTime{ get; set; }

        [DataType(DataType.Text)]
        public string DetailErrorInfo { get; set; }
    }
}
