using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolf.Vecc.Data.AuthCore;
using Wolf.Vecc.IService.ISysService;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.Controllers
{
    public class IndexController : BaseController
    {
        private readonly ISysUserService _userService;

        public IndexController()
        {
        }

        public IndexController(ISysUserService userService)
        {
            _userService = userService;
        }

        [VeccAuthorize(Roles = "admin")]
        public ActionResult VeccIndex()
        {
            var id = WorkUser.UserId;
            var name = WorkUser.UserName;
            ViewData["id"] = id.ToString();
            ViewData["name"] = name;
            return View();
        }

        [VeccAuthorize(Roles = "sgs,engineer")]
        public ActionResult OtherIndex()
        {
            var id = WorkUser.UserId;
            var name = WorkUser.UserName;
            ViewData["id"] = id.ToString();
            ViewData["name"] = name;
            return View();
        }

        public ActionResult Index()
        {
            if (WorkUser != null)
            {
                if (WorkUser.RoleId == 1)
                {
                    //获取缓存
                    return RedirectToAction("VeccIndex", "Index");
                }
                else
                {
                    return RedirectToAction("OtherIndex", "Index");
                }
            }
            return RedirectToAction("Login", "Account");
        }
    }
}