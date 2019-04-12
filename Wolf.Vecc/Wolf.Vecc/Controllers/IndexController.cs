using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        // GET: Index
        public ActionResult Index()
        {
            //List<SysUser> users = _userService.All();
            return View();
        }
    }
}