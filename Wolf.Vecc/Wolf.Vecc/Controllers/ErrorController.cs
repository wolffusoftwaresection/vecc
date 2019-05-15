using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wolf.Vecc.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        // GET: Error
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        //[NoOnAction]
        public ActionResult HttpError()
        {
            return View("Error");
        }

        //[NoOnAction]
        public ActionResult NotFound()
        {
            return View();
        }
    }
}