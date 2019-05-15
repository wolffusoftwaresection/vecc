using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Wolf.Vecc.Comm.ExceptionExtent;
using Wolf.Vecc.Comm.Helpers;
using Wolf.Vecc.Comm.NlogExtent;

namespace Wolf.Vecc.Filters
{
    public class ExceptionFilter : HandleErrorAttribute, IExceptionFilter
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //var exception = filterContext.Exception;
            //string sbExMsg = ExceptionExt.ExceptionMsg(exception);

            //#region Log03 参数名或参数值进入时错误日志记录
            //LogEventInfo ei = new LogEventInfo(LogLevel.Error, sbExMsg, "");
            //var erroraddr = "异常位置: " + filterContext.RequestContext.HttpContext.Request.Url.ToString();
            //var errorinfo = "异常信息: " + filterContext.Exception.Message;
            //var errortrace = "堆栈信息:" + filterContext.Exception.StackTrace;
            //string controllerName = (string)filterContext.RouteData.Values["controller"];
            //string actionName = (string)filterContext.RouteData.Values["action"];
            //ei.Properties["stacktrace"] = errortrace;
            //ei.Properties["controller"] = controllerName;
            //ei.Properties["action"] = actionName;
            //ei.Properties["loggeruser"] = HttpContext.Current.User.Identity.Name;
            //ei.Properties["param"] = erroraddr + errorinfo;
            //LogHelper.LogException(ei);
            //#endregion

            //告诉MVC框架异常被处理
            filterContext.HttpContext.Response.WriteFile("~/ErrorPage.html");
            //跳转到自定义的错误页,增强用户体验。
            //ActionResult result = new ViewResult() { ViewName = "HttpError" };
            //filterContext.Result = result;
            //异常处理结束后,一定要将ExceptionHandled设置为true,否则仍然会继续抛出错误。
            filterContext.ExceptionHandled = true;
        }
    }
}