using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Wolf.Vecc.App_Start;
using Wolf.Vecc.Data.AuthCore.AuthExt;
using Wolf.Vecc.Data.AuthCore.PrincipalExt;
using Wolf.Vecc.Data.DataContext;

namespace Wolf.Vecc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DatabaseInitializer.Initialize();
            AutofacConfig.Register();
        }

        protected void Application_PostAuthenticateRequest(object sender, System.EventArgs e)
        {
            if (HttpContext.Current.User.Identity is FormsIdentity formsIdentity && formsIdentity.IsAuthenticated && formsIdentity.AuthenticationType == "Forms")
            {
                HttpContext.Current.User =
                    VeccFormsAuthentication<VeccUserDataPrincipal>.TryParsePrincipal(HttpContext.Current.Request);
            }
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();
        //    //Server.ClearError();
        //    //跳转到指定的自定义错误页
        //    Response.Redirect("/Error/HttpError");
        //}

        //protected void Application_EndRequest()
        //{
        //Exception lastError = Server.GetLastError();
        //if (lastError != null)
        //{
        //异常信息
        //string strExceptionMessage = string.Empty;
        // Server.ClearError();
        //Server.Transfer("~/Error/NotFound.html");
        //return;
        //对HTTP 404做额外处理，其他错误全部当成500服务器错误
        //HttpException httpError = lastError as HttpException;
        //if (httpError != null)
        //{
        //    //获取错误代码
        //    int httpCode = httpError.GetHttpCode();
        //    strExceptionMessage = httpError.Message;
        //    if (httpCode == 400 || httpCode == 404)
        //    {
        //        Response.StatusCode = 404;
        //        //跳转到指定的静态404信息页面，根据需求自己更改URL
        //        //Response.RedirectToRoute("Default", new { controller = "Error", action = "HttpError" });
        //        Response.WriteFile("~/Error/NotFound.html");
        //        Server.ClearError();
        //        return;
        //    }
        //}
        //}
        //}
    }
}
