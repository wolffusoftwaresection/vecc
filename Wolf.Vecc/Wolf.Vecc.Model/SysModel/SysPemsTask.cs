using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model.SysModel
{
    [Table("Sys_PemsTask")]
    public class SysPemsTask
    {
        [Column("pems_task_id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PemsTaskId { get;set;}

        [Column("task_id")] 
        public Guid TaskId { get; set; }

        [Column("vehicle_model")]
        [Display(Name = "整车型号")]
        [StringLength(50)]
        public string VehicleModel { get; set; }

        [Column("vehicle_type")]
        [Display(Name = "整车类别")]
        [StringLength(50)]
        public string VehicleType { get; set; }

        [Column("pems_factory")]
        [Display(Name = "PEMS厂商")]
        [StringLength(50)]
        public string PemsFactory { get; set; }

        [Column("whtc_power")]
        [Display(Name = "WHTC循环功")]
        public float WhtcPower { get; set; }

        [Column("vehicle_quality")]
        [Display(Name = "整备质量")]
        public float VehicleQuality { get; set; }

        [Column("max_ref_torque")]
        [Display(Name = "最大参考扭矩")]
        public float MaxRefTorque { get; set; }

        [Column("max_quality")]
        [Display(Name = "最大质量")]
        public float MaxQuality { get; set; }

        [Column("max_power")]
        [Display(Name = "最大功率")]
        public float MaxPower { get; set; }

        [Column("percentage_load")]
        [Display(Name = "载货百分比")]
        public float PercentageLoad { get; set; }

        [Column("data_url")]
        [Display(Name = "数据文件url")]
        [DataType(DataType.Text)]
        public string DataUrl { get; set; }

        [Column("kr_nox")]
        [StringLength(50)]
        public string KrNox { get; set; }
        
    }
}
