using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model.SysModel
{
    [Description("日志异常类")]
    [Table("Sys_Log")]
    public class SysLog
    {
        [Key]
        [Display(Name = "日志ID")]
        public int ID { set; get; }

        [Display(Name = "请求用时")]
        [StringLength(500)]
        public string Time_Stamp { set; get; }

        [Display(Name = "日志级别")]
        [StringLength(50)]
        public string Level { set; get; }

        [Display(Name = "机器名")]
        [StringLength(500)]
        public string Host { set; get; }

        [Display(Name = "日志类型")]
        [StringLength(500)]
        public string type { set; get; }

        [Display(Name = "来源")]
        [DataType(DataType.Text)]
        public string source { set; get; }

        [Display(Name = "记录人")]
        [StringLength(500)]
        public string logger { set; get; }

        [Display(Name = "控制器")]
        [StringLength(500)]
        public string controller { set; get; }

        [Display(Name = "ACtion")]
        [StringLength(500)]
        public string action { set; get; }

        [Display(Name = "操作人")]
        [StringLength(500)]
        public string loggeruser { set; get; }

        [Display(Name = "参数")]
        [DataType(DataType.Text)]
        public string param { set; get; }

        [Display(Name = "日志内容")]
        [DataType(DataType.Text)]
        public string message { set; get; }

        [Display(Name = "异常跟踪")]
        [DataType(DataType.Text)]
        public string stacktrace { set; get; }
    }
}
