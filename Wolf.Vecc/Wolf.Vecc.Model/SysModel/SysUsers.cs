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
    [Description("用户表")]
    [Table("Sys_User")]
    public class SysUsers : BaseModel<int>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Column("user_name")]
        [StringLength(120)]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Column("pass_word")]
        [StringLength(300)]
        [Display(Name = "用户密码")]
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column("email")]
        [StringLength(350)]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [Column("country")]
        [StringLength(120)]
        [Display(Name = "国家")]
        public string Country { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Column("phone")]
        [StringLength(50)]
        [Display(Name = "手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        [Column("enterprise_name")]
        [StringLength(350)]
        [Display(Name = "企业名称")]
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 所属角色id
        /// </summary>
        [Column("role_id")]
        [Display(Name = "所属角色id")]
        public int RoleId { get; set; }

        /// <summary>
        /// 账号状态(审核状态)1待审批 ，2未通过，3审批通过当为3时为审批通过可登陆
        /// </summary>
        [Column("account_status")]
        [Display(Name = "账号状态")]
        public int AccountStatus { get; set; }

        /// <summary>
        /// 注册或创建时间
        /// </summary>
        [Column("create_date")]
        [Display(Name = "注册或创建时间")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 用户类型（1是工程师注册 2为vecc添加的企业用户 0为veccadmin）
        /// </summary>
        [Column("user_type")]
        [Display(Name = "用户类型")]
        public int UserType { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }
}