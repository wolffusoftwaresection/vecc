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
            System.Timers.Timer timer = new System.Timers.Timer(5000);
            AsyncManager.OutstandingOperations.Increment();
            timer.Elapsed += (sender, e) =>
            {
                AsyncManager.Parameters["now"] = _sysUserService.GetApprovalNumber().ToString();
                AsyncManager.OutstandingOperations.Decrement();
            };
            //启动计时器
            timer.Start();
        }

        public ActionResult ApprovalPollingCompleted(string now)
        {
            return Json(new
            {
                d = now
            },JsonRequestBehavior.AllowGet);
        }
    }
}