﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Wolf.Vecc.Comm.Excel;
using Wolf.Vecc.Comm.Helpers;
using Wolf.Vecc.Comm.TableDictionary;
using Wolf.Vecc.Data.AuthCore;
using Wolf.Vecc.IService.IReadFileService;
using Wolf.Vecc.IService.ISysService;
using Wolf.Vecc.Model.ResultModel;
using Wolf.Vecc.Model.SysModel;
using Wolf.Vecc.Model.ViewModel;

namespace Wolf.Vecc.Controllers
{
    [VeccAuthorize(Roles = "admin,sgs,engineer")]
    public class CalculationController : BaseController
    {
        private readonly ISysTaskService _sysTaskService;
        private readonly ISysPemsTaskService _sysPemsTaskService;
        private readonly ISysTaskResultService _sysTaskResultService;
        private readonly IReadRstService _readRstService;

        public CalculationController()
        {
            
        }
        public CalculationController(ISysTaskService sysTaskService, ISysPemsTaskService sysPemsTaskService, IReadRstService readRstService, ISysTaskResultService sysTaskResultService)
        {
            _sysTaskService = sysTaskService;
            _sysPemsTaskService = sysPemsTaskService;
            _readRstService = readRstService;
            _sysTaskResultService = sysTaskResultService;
        }
        // GET: Calculation
        public ActionResult Index()
        {
            ViewBag.Id = Guid.NewGuid().ToString();
            return View();
        }

        /// <summary>
        /// 检验报告模板下载
        /// </summary>
        public void CreatCheckExcelTemplate()
        {
            ExcelHelper execlHelper = new ExcelHelper();
            List<ExcelSelectList> selectList = new List<ExcelSelectList>();
            MemoryStream ms = execlHelper.CreatExcelModelTemplate();
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode("检测报告模板", Encoding.UTF8)));
            Response.BinaryWrite(ms.ToArray());
            Response.End();
            ms.Close();
            ms.Dispose();
        }

        public ActionResult PemsCalculationResultsView(string taskId)
        {
            //var data = _readRstService.ReadTestInfo(Server.MapPath(Result_Root_Url), taskId);//测试
            //var _json = JsonHelper.SerializeDictionaryToJsonString(data);
            //var pemsTask = _sysPemsTaskService.GetSysPemsTaskByTaskId(taskId);
            //var taskResult = _sysTaskResultService.GetTaskResultByTaskId(taskId);

            var data = _readRstService.ReadTestInfo(Server.MapPath(Result_Root_Url), "b88859c7-dee8-4ec0-bcd5-90c9fa6a71e7");//测试
            var _json = JsonHelper.SerializeDictionaryToJsonString(data);
            var pemsTask = _sysPemsTaskService.GetSysPemsTaskByTaskId("4c668a4b-dcfd-4ac8-94ff-8512e5fb4c0f");
            var taskResult = _sysTaskResultService.GetTaskResultByTaskId("4c668a4b-dcfd-4ac8-94ff-8512e5fb4c0f");

            var tasks = new TaskResultModel
            {
                RouteDescription = taskResult.RouteDescription,
                TaskId = pemsTask.TaskId.ToString(),
                TestDate = taskResult.TestDate,
                PlaceTest = taskResult.PlaceTest,
                TestPerson = taskResult.TestPerson,
                TestTime = taskResult.TestTime,
                VehicleType = pemsTask.VehicleType,
                VehicleModel = pemsTask.VehicleModel,
                WhtcPower = pemsTask.WhtcPower.ToString()
            };

            ViewBag.Result = _json;
            return View(tasks);

            //return View();
        }

        public ActionResult PemsCalculationResults(string taskId)
        {
            var data = _readRstService.ReadTestInfo(Server.MapPath(Result_Root_Url), "b88859c7-dee8-4ec0-bcd5-90c9fa6a71e7");//测试
            var _json = JsonHelper.SerializeDictionaryToJsonString(data);
            var pemsTask = _sysPemsTaskService.GetSysPemsTaskByTaskId("4c668a4b-dcfd-4ac8-94ff-8512e5fb4c0f");
            var taskResult = _sysTaskResultService.GetTaskResultByTaskId("4c668a4b-dcfd-4ac8-94ff-8512e5fb4c0f");

            var tasks = new TaskResultModel
            {
                RouteDescription = taskResult.RouteDescription,
                TaskId = pemsTask.TaskId.ToString(),
                TestDate = taskResult.TestDate,
                PlaceTest = taskResult.PlaceTest,
                TestPerson = taskResult.TestPerson,
                TestTime = taskResult.TestTime,
                VehicleType = pemsTask.VehicleType,
                VehicleModel = pemsTask.VehicleModel,
                WhtcPower = pemsTask.WhtcPower.ToString()
            };

            ViewBag.Result = _json;
            return View(tasks);
        }

        public ActionResult ImportRarData(string id, string vehicleModel, string pemsFactory, string vehicleType, string whtcPower,
            string vehicleQuality, string maxRefTorque, string maxQuality, string percentageLoad, string maxPower, string nox)
        {
            //ViewBag.id = id;
            //ViewBag.vehicleModel = vehicleModel;
            //ViewBag.pemsFactory = pemsFactory;
            //ViewBag.vehicleType = vehicleType;
            //ViewBag.whtcPower = whtcPower;
            //ViewBag.vehicleQuality = vehicleQuality;
            //ViewBag.maxRefTorque = maxRefTorque;
            //ViewBag.maxQuality = maxQuality;
            //ViewBag.percentageLoad = percentageLoad;
            //ViewBag.maxPower = maxPower;
            //ViewBag.nox = nox;

            CalculationModel calculationModel = new CalculationModel
            {
                Id = id,
                MaxPower = maxPower,
                MaxQuality = maxQuality,
                MaxRefTorque = maxRefTorque,
                Nox = nox,
                PemsFactory = pemsFactory,
                PercentageLoad = percentageLoad,
                VehicleModel = vehicleModel,
                VehicleQuality = vehicleQuality,
                VehicleType = vehicleType,
                WhtcPower = whtcPower
            };

            return View(calculationModel);
        }

        public ActionResult CalculationView(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public string UploadExcel()
        {
            //整个验证是否有错误，没有返回true
            bool flag = false;
            //判断表头和模板是否一致，一致true
            bool headIsRight = false;
            DataTable newDT = null;
            int num = Request.Files.Count;
            if (num > 0)
            {
                ExcelHelper execlHelper = new ExcelHelper();
                Stream stream = Request.Files[0].InputStream;
                //将execl转换成DataTable
                DataTable dt = execlHelper.ExcelToTable(stream, TableDictionaries.ReportTemplateHeadList, Request.Files[0].FileName, ref headIsRight, "检验报告基本参数");
                //判断表头和模板是否一致
                if (headIsRight && dt != null)
                {
                    //合法性验证
                    newDT = UploadExcelCommon.UploadExcel(dt, null, ref flag);
                    UploadExcelCommon.dt = newDT;
                }
            }
            return JsonConvert.SerializeObject(new { message = "", flag = flag, headIsRight = headIsRight });
        }

        public ActionResult ImportExcelView()
        {
            UploadExcelCommon.dt = new DataTable();
            return View();
        }

        [HttpPost]
        public JsonResult ImportDataing(IEnumerable<HttpPostedFileBase> filesInput, CalculationModel calculationModel)
        {
            string url = "";
            var _file = System.Web.HttpContext.Current.Request.Files[0];
            if (_file.FileName != "")
            {
                UpFileExt.CreateFolder(calculationModel.Id);
                //返回的文件所在路径（文件名）
                url = UpFileExt.UpLoadFile(_file, calculationModel.Id + "/OriginalFile");
                RarOperatorExt.UnRAR(Server.MapPath("../UploadFiles/" + calculationModel.Id + "/OriginalFile/" + url), Server.MapPath("../UploadFiles/" + calculationModel.Id + "/OriginalFile"), "");
                // rarOperator.UnRAR(Server.MapPath("UploadFile/25314825-f968-4311-94b1-c2b02a8550c2/OriginalFile/test.rar"), Server.MapPath("UploadFile/25314825-f968-4311-94b1-c2b02a8550c2/OriginalFile"), "");
                System.IO.File.Copy(Server.MapPath("../TemplateFile/reportdata.xlsx"), Server.MapPath("../UpLoadFiles/" + calculationModel.Id + "/ResultFiles/reportdata.xlsx"), true);
            }
            if (url != "")
            {
                var result = false;
                using (TransactionScope transaction = new TransactionScope())
                {
                    SysTask sysTask = new SysTask
                    {
                        TaskId = new Guid(calculationModel.Id),
                        TaskType = 0,
                        TaskStatus = 4,
                        CreateTime = DateTime.Now,
                        UserId = WorkUser.UserId
                    };
                    if (_sysTaskService.Insert(sysTask) > 0)
                    {
                        SysPemsTask pemsTask = new SysPemsTask
                        {
                            TaskId = new Guid(calculationModel.Id),
                            PemsTaskId = Guid.NewGuid(),
                            MaxQuality = Convert.ToDouble(calculationModel.MaxQuality),
                            MaxRefTorque = Convert.ToDouble(calculationModel.MaxRefTorque),
                            VehicleQuality = Convert.ToDouble(calculationModel.VehicleQuality),
                            WhtcPower = Convert.ToDouble(calculationModel.WhtcPower),
                            PercentageLoad = Convert.ToDouble(calculationModel.PercentageLoad),
                            PemsFactory = calculationModel.PemsFactory,
                            VehicleType = calculationModel.VehicleType,
                            VehicleModel = calculationModel.VehicleModel,
                            DataUrl = url.Substring(0, url.LastIndexOf(".")) + ".xls",
                            MaxPower = Convert.ToDouble(calculationModel.MaxPower),
                            KrNox = calculationModel.Nox
                        };
                        if (_sysPemsTaskService.Insert(pemsTask) > 0)
                        {
                            result = true;
                        }
                    }
                    transaction.Complete();
                }
                if (result == true)
                {
                    return Success("上传测试数据成功！");
                }
                else
                {
                    return Failure("上传测试数据失败！");
                }
            }
            return Failure("上传测试数据失败!");
        }
    }
}