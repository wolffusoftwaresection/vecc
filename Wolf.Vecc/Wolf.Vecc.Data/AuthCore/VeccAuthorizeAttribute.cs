using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Wolf.Vecc.Data.AuthCore.PrincipalExt;

namespace Wolf.Vecc.Data.AuthCore
{
    public class VeccAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                base.OnAuthorization(filterContext);
            else
                filterContext.Result = new RedirectResult("~/Account/Login");
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (httpContext.User is VeccFormsPrincipal<VeccUserDataPrincipal> user)
                return (user.IsInRole(Roles) || user.IsInUser(Users));

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //验证不通过,直接跳转到相应页面，注意：如果不使用以下跳转，则会继续执行Action方法
            filterContext.Result = new RedirectResult("http://www.baidu.com");
        }
    }
}
