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
    [Description("用户上传数据审批表")]
    [Table("Sys_Data")]
    public class SysData : BaseModel<int>
    {
        /// <summary>
        /// 上传用户id
        /// </summary>
        [Column("user_id")]
        [Display(Name = "上传用户id")]
        public int UserId { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [Column("data_name")]
        [Display(Name = "文件名称")]
        [StringLength(600)]
        public string DataName { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        [Column("data_url")]
        [Display(Name = "文件地址")]
        [StringLength(600)]
        public string DataUrl { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [Column("data_status")]
        [Display(Name = "审核状态")]
        public int DataStatus { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        [Column("is_public")]
        [Display(Name = "是否公开")]
        public int IsPublic { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        [Column("upload_date")]
        [Display(Name = "上传时间")]
        public DateTime UploadDate { get; set; }
    }
}
