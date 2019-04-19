using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Wolf.Vecc.IService.ISysService;

namespace Wolf.Vecc.Data.AuthCore.PrincipalExt
{
    public class VeccUserDataPrincipal : IPrincipal
    {
        private readonly IAccountService _authService; 
        public VeccUserDataPrincipal()
        {
            _authService = DependencyResolver.Current.GetService<IAccountService>();
        }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Phone { get; set; }

        public string EnterpriseName { get; set; }

        public int AccountStatus { get; set; }

        public int UserType { get; set; }

        //这里可以定义其他一些属性
        public int RoleId { get; set; }

        //当使用Authorize特性时，会调用改方法验证角色 
        public bool IsInRole(string role)
        {
            //找出用户所有所属角色
            //var userroles = mingshiDb.UserRole.Where(u => u.UserId == UserId).Select(u => u.Role.RoleName).ToList();
            //var userRoleIds = _authService.GetRoleIdByUserId(UserId);
            //获取用户角色编号对应的角色名称
            var userRoleName = _authService.GetRoleNamesByRoleIds(RoleId);
            var roles = role.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            return (from s in roles from userrole in userRoleName where s.Equals(userrole) select s).Any();
        }

        //验证用户信息
        public bool IsInUser(string user)
        {
            //找出用户所有所属角色
            var users = user.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return _authService.GetUsers().Any(u => users.Contains(u.UserName));
        }


        [ScriptIgnore]    //在序列化的时候忽略该属性
        public IIdentity Identity { get { throw new NotImplementedException(); } }
    }
}
