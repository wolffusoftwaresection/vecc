using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Wolf.Vecc.Comm.Helpers;
using Wolf.Vecc.IService.ISysService;
using Wolf.Vecc.Model.SysModel;
using Wolf.Vecc.Model.ViewModel;

namespace Wolf.Vecc.Controllers
{
    public class CalculationController : BaseController
    {
        private readonly ISysTaskService _sysTaskService;
        private readonly ISysPemsTaskService _sysPemsTaskService;
        public CalculationController(ISysTaskService sysTaskService, ISysPemsTaskService sysPemsTaskService)
        {
            _sysTaskService = sysTaskService;
            _sysPemsTaskService = sysPemsTaskService;
        }
        // GET: Calculation
        public ActionResult Index()
        {
            ViewBag.Id = Guid.NewGuid().ToString();
            return View();
        }

        public ActionResult PemsCalculationResults()
        {
            return View();
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
                //System.IO.File.Copy(Server.MapPath("../TemplateFile/reportdata.xlsx"), Server.MapPath("../UpLoadFiles/" + calculationModel.Id + "/ResultFiles/reportdata.xlsx"), true);
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