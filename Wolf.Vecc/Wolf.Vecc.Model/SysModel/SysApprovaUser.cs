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
    [Description("vecc账号审批记录 人员的审批")]
    [Table("Sys_Approva_User")]
    public class SysApprovaUser : BaseModel<int>
    { /// <summary>
      /// vecc审批人账号
      /// </summary>
        [Column("vecc_user_id")]
        [Display(Name = "vecc审批人账号")]
        public int VeccUserId { get; set; }

        /// <summary>
        /// 被审批的用户id
        /// </summary>
        [Column("user_id")]
        [Display(Name = "被审批的用户id")]
        public int UserId { get; set; }

        /// <summary>
        /// vecc账号审批时填写的备注信息
        /// </summary>
        [Column("approval_remark")]
        [StringLength(800)]
        [Display(Name = "vecc账号审批时填写的备注信息")]
        public string ApprovalRemark { get; set; }

        /// <summary>
        /// 账号状态 审核状态表 状态只要2未通过 3通过 
        /// </summary>
        [Column("account_status")]
        [Display(Name = "账号状态 审核状态表 状态只要2未通过 3通过 ")]
        public int AccountStatus { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Column("approval_date")]
        [Display(Name = "审核时间")]
        public DateTime ApprovalDate { get; set; }
    }
}
