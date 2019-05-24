using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Comm.Helpers;
using Wolf.Vecc.Data.DataContext;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.Data.Initialize
{
    public class InitData : CreateDatabaseIfNotExists<VeccContext>
    {
        protected override void Seed(VeccContext context)
        {
            //初始化用户数据 没有数据库时执行一次
            string salt;
            var code = UtilityHelper.CreateHashCodePW(GlobalConfigHelper.GetAdminInitialPassword(), out salt);
            SysUsers user = new SysUsers { UserName = "veccadmin", RoleId = 1, Email="veccadmin@126.com", Country= "中国", AccountStatus = 3, UserType = 0, CreateDate = DateTime.Now, IsDel = 0, Password = code, Salt = salt };

            //角色基础数据
            List<SysRole> sysRoles = new List<SysRole> {
                new SysRole { RoleDescribe = "vecc管理员", RoleName = "admin", IsDel = 0 },
                new SysRole { RoleDescribe = "sgs检测机构", RoleName = "sgs", IsDel = 0 },
                new SysRole { RoleDescribe = "engineer工程师", RoleName = "engineer", IsDel = 0 }
            };

            //基础参数设置
            List<SysParams> sysParams = new List<SysParams> {
                new SysParams { ParamNumber = "001", ParamName = "数据审批状态", ParamType="pending", ParamValue=1 ,IsDel = 0 },
                new SysParams { ParamNumber = "001", ParamName = "数据审批状态", ParamType="fail", ParamValue=2 ,IsDel = 0 },
                new SysParams { ParamNumber = "001", ParamName = "数据审批状态", ParamType="passed", ParamValue=3 ,IsDel = 0 },
                new SysParams { ParamNumber = "002", ParamName = "用户审批状态", ParamType="pending", ParamValue=1 ,IsDel = 0 },
                new SysParams { ParamNumber = "002", ParamName = "用户审批状态", ParamType="fail", ParamValue=2 ,IsDel = 0 },
                new SysParams { ParamNumber = "002", ParamName = "用户审批状态", ParamType="passed", ParamValue=3 ,IsDel = 0 }
            };

            //保存
            context.SysUser.Add(user);
            context.SysRole.AddRange(sysRoles);
            context.SysParams.AddRange(sysParams);
            context.SaveChanges();
        }
    }
}
