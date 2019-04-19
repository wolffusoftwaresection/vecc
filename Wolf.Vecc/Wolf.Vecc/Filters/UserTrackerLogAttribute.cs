using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Wolf.Vecc.Comm.Helpers;

namespace Wolf.Vecc.Filters
{
    public class UserTrackerLogAttribute : ActionFilterAttribute, IActionFilter
    {
        protected string _errorMsg = string.Empty;
        [AllowAnonymous]
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                #region Log01 参数名或参数值
                StringBuilder sb = new StringBuilder();
                sb.Append("PortName:" + filterContext.ActionDescriptor.ActionName);//获得调用接口
                sb.AppendLine();
                sb.Append("StatusCode:" + Convert.ToInt32(new HttpResponseMessage(HttpStatusCode.OK).StatusCode).ToString());//设置状态码
                sb.AppendLine();
                sb.Append("ClientIp:" + UtilityHelper.GetUserIp().ToString());
                sb.AppendLine();
                sb.Append("params:" + JsonHelper.ToJson(filterContext.ActionParameters));
                sb.AppendLine();
                sb.Append("OnActionExecuting-user:" + HttpContext.Current.User.Identity.Name); 
                LogHelper.LogInfo(sb.ToString());
                #endregion
                //string error = string.Empty;
                //foreach (var key in filterContext.Controller.ViewData.Keys)
                //{
                //    var state = filterContext.Controller.ViewData.ModelState[key];
                //    if (state.Errors.Any())
                //    {
                //        error = state.Errors.First().ErrorMessage;
                //        if (error == "")
                //        {
                //            error += state.Errors.First().Exception.Message ?? "";
                //        }
                //        #region Log03 参数名或参数值进入时错误日志记录
                //        //log01 type=db
                //        LogEventInfo ei = new LogEventInfo(LogLevel.Error, "action_outside请查看错误日志", "");
                //        ei.Properties["stacktrace"] = error;
                //        ei.Properties["controller"] = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                //        ei.Properties["action"] = filterContext.ActionDescriptor.ActionName;
                //        ei.Properties["loggeruser"] = HttpContext.Current.User.Identity.Name;
                //        ei.Properties["param"] = JsonHelper.ToJson(filterContext.ActionParameters);
                //        LogHelper.LogException(ei);
                //        #endregion
                //    }
                //}
            }
            base.OnActionExecuting(filterContext);
        }

        [AllowAnonymous]
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //#region Log01 接口被访问记录
            ////log02 type=file
            StringBuilder sb = new StringBuilder();
            sb.Append("PortName:" + filterContext.ActionDescriptor.ActionName);//获得调用接口
            sb.AppendLine();
            sb.Append("StatusCode:" + Convert.ToInt32(new HttpResponseMessage(HttpStatusCode.OK).StatusCode).ToString());//设置状态码
            sb.AppendLine();
            sb.Append("ClientIp:" + UtilityHelper.GetUserIp().ToString());
            sb.AppendLine();
            //sb.Append("params:" + JsonHelper.ToJson(filterContext.RequestContext.HttpContext));
            sb.AppendLine();
            sb.Append("OnActionExecuted-user:" + HttpContext.Current.User.Identity.Name);
            LogHelper.LogInfo(sb.ToString());
            //#endregion
            base.OnActionExecuted(filterContext);
            bool result = false;
            if (filterContext.Exception != null)
            {
                _errorMsg = "请查看错误日志";
                if (filterContext.Exception.GetType().Name == "DbUpdateConcurrencyException")
                {
                    var e = filterContext.Exception as DbUpdateConcurrencyException;
                    var entry = e.Entries.Single();
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null) { _errorMsg = "无法保存更改，系已经被其他用户删除."; }
                    else { _errorMsg = "无法保存更改，当前记录已经被其他人更改."; }
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Success = false,
                            Msg = _errorMsg
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    result = true;
                }
                else
                {
                    //不是并发异常记录日志
                    #region Log02 内部错误日志记录
                    //log02 type=db
                    LogEventInfo ei = new LogEventInfo(LogLevel.Error, "action_inside" + _errorMsg, sb.ToString());
                    ei.Properties["stacktrace"] = filterContext.Exception.StackTrace;
                    ei.Properties["controller"] = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    ei.Properties["action"] = filterContext.ActionDescriptor.ActionName;
                    ei.Properties["loggeruser"] = HttpContext.Current.User.Identity.Name;
                    ei.Properties["param"] = JsonHelper.ToJson(filterContext.ActionDescriptor.GetParameters());
                    LogHelper.LogException(ei);
                    #endregion
                    //filterContext.Result = new RedirectResult("~/Account/Login");
                }
            }
            base.OnActionExecuted(filterContext);
            filterContext.ExceptionHandled = result;
        }
    }
}