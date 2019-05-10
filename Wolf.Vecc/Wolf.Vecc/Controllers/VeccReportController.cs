﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolf.Vecc.IService.IVeccReportService;

namespace Wolf.Vecc.Controllers
{
    [Authorize]
    public class VeccReportController : BaseController
    {
        private readonly IReportService _reportService;

        public VeccReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        // GET: VeccReport
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 用户每月注册数量统计view
        /// </summary>
        public ActionResult ReportUserMonthRegisterView()
        {
            return View();
        }

        public JsonResult ReportUserMonthRegister(string year)
        {
            return Json(new { data = _reportService.GetReportUserMonthRegister(year) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReportUserMonthRegisterInfo(string year)
        {
            return Json(new { data = _reportService.ReportUserMonthRegisterInfo(year) }, JsonRequestBehavior.AllowGet);
        }
    }
}