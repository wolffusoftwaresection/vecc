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
    [Description("系统参数数据审批状态 1：待审批 2：未通过 3：已通过人员审批状态  1：待审批 ，2：未通过，3：已通过")]
    [Table("Sys_Params")]
    public class SysParams:BaseModel<int>
    {
        /// <summary>
        /// 参数编号
        /// </summary>
        [Column("param_number")]
        [Display(Name = "参数编号")]
        [StringLength(20)]
        public string ParamNumber { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        [Column("param_type")]
        [Display(Name = "参数类型")]
        [StringLength(200)]
        public string ParamType { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        [Column("param_name")]
        [Display(Name = "类型名称")]
        [StringLength(100)]
        public string ParamName { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        [Column("param_value")]
        [Display(Name = "参数值")]
        public int ParamValue { get; set; }
        
    }
}
