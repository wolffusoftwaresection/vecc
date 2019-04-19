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
        public IndexController(ISysUserService userService)
        {
            _userService = userService;
        }

        [VeccAuthorize(Roles = "admin,sgs")]
        public ActionResult Index()
        {
            var id = WorkUser.UserId;
            var name = WorkUser.UserName;
            ViewData["id"] = id.ToString();
            ViewData["name"] = name;
            return View();
        }
    }
}