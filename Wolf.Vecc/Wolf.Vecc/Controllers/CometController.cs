using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolf.Vecc.IService.ISysService;

namespace Wolf.Vecc.Controllers
{
    public class CometController : AsyncController
    {
        private readonly ISysUserService _sysUserService;
        public CometController(ISysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }

        // GET: Comet
        public ActionResult Index()
        {
            return View();
        }

        public void ApprovalPollingAsync()
        {
            //计时器，10分钟触发一次Elapsed事件600000
            System.Timers.Timer timer = new System.Timers.Timer(5000);
            AsyncManager.OutstandingOperations.Increment();
            timer.Elapsed += (sender, e) =>
            {
                AsyncManager.Parameters["now"] = _sysUserService.GetApprovalNumber().ToString();
                //告诉ASP.NET异步操作已完成，进行LongPollingCompleted方法的调用
                AsyncManager.OutstandingOperations.Decrement();
            };
            //启动计时器
            timer.Start();
        }

        //LongPolling Action 2 - 异步处理完成，向客户端发送响应
        public ActionResult ApprovalPollingCompleted(string now)
        {
            return Json(new
            {
                d = now
            },JsonRequestBehavior.AllowGet);
        }
    }
}