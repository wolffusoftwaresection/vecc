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
    [Description("用户角色表")]
    [Table("Sys_Role")]
    public class SysRole : BaseModel<int>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Column("role_name")]
        [Display(Name = "角色名称")]
        [StringLength(100)]
        public string RoleName { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        [Column("role_describe")]
        [Display(Name = "角色描述")]
        [StringLength(255)]
        public string RoleDescribe { get; set; }
    }
}
