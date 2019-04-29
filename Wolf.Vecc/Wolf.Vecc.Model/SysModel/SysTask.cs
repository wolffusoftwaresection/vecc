using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model.SysModel
{
    [Table("Sys_Task")]
    public class SysTask
    {
        [Column("task_id")]
        [Key]
        public Guid TaskId { get; set; }

        [Column("create_time")]
        public DateTime? CreateTime { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("task_status")]
        [Display(Name = "任务状态，0表示等待处理，1表示正在处理，2表示成功处理完成，3表示处理失败")]
        public int TaskStatus { get; set; }

        [Column("task_type")]
        [Display(Name = "任务类型，0表示pems任务，1表示rde任务")] 
        public int TaskType { get; set; }
    }
}
