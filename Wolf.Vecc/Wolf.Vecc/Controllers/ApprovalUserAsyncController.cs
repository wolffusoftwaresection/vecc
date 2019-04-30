using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolf.Vecc.IService.ISysService;

namespace Wolf.Vecc.Controllers
{
    public class ApprovalUserAsyncController : AsyncController
    {
        private readonly ISysUserService _sysUserService;
        public ApprovalUserAsyncController(ISysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }
        // GET: ApprovalUserAsync
        #region 新用户注册实时数量
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
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}