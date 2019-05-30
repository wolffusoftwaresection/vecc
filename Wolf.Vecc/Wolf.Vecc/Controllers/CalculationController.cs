
using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
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
        private readonly ISysDataService _sysDataService;

        public CalculationController()
        {
            
        }
        public CalculationController(ISysDataService sysDataService, ISysTaskService sysTaskService, ISysPemsTaskService sysPemsTaskService, IReadRstService readRstService, ISysTaskResultService sysTaskResultService)
        {
            _sysTaskService = sysTaskService;
            _sysPemsTaskService = sysPemsTaskService;
            _readRstService = readRstService;
            _sysTaskResultService = sysTaskResultService;
            _sysDataService = sysDataService;
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
        public void CreatCheckTxtTemplate()
        {
            string fileName = "检测报告模板.txt";//客户端保存的文件名
            string filePath = Server.MapPath("~/检测报告模板.txt");//路径

            //以字符流的形式下载文件
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        public ActionResult PemsCalculationResultsViewDown()
        {
            return View();
        }

        public ActionResult PemsCalculationResultsView(string taskId)
        {
            //var data = _readRstService.ReadTestInfo(Server.MapPath(Result_Root_Url), taskId);//测试
            //var _json = JsonHelper.SerializeDictionaryToJsonString(data);
            //var pemsTask = _sysPemsTaskService.GetSysPemsTaskByTaskId(taskId);
            //var taskResult = _sysTaskResultService.GetTaskResultByTaskId(taskId);

            var data = _readRstService.ReadTestInfo(Server.MapPath(Result_Root_Url), "b88859c7-dee8-4ec0-bcd5-90c9fa6a71e7");//测试
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var _json = JsonHelper.SerializeDictionaryToJsonString(data);
            //var _calculationResultData = js.ConvertToType<CalculationResultData>(_json);
            
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
            //    calculationResultData = _calculationResultData,
            //    loadResultModel = 
            };
            //ViewData["Result"] = _json;
            //TempData["Result"] = _json;
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
            string vehicleQuality, string maxRefTorque, string maxQuality, string percentageLoad, string maxPower, string nox, string emissionStandard)
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
                WhtcPower = whtcPower,
                EmissionStandard = emissionStandard
            };

            return View(calculationModel);
        }
 
        [HttpPost]
        public ActionResult LoadResultModel(string taskId)
        {
            var file = Request.Files[0];
            byte[] byts = new byte[file.InputStream.Length];
            file.InputStream.Read(byts, 0, byts.Length);
            var requestContent = Encoding.Default.GetString(byts);
            string[] array = requestContent.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int totalCount = array.Length; // 导入的记录总数 
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Trim() != "检验报告基本参数" || array[i].Trim() != "检验结论" || array[i].Trim() != "检验车辆基本参数")
                {
                    string[] arraynamevalue = array[i].Split('=');
                    if (arraynamevalue.Length == 2)
                    {
                        keyValuePairs.Add(arraynamevalue[0], arraynamevalue[1] == "" ? "—" : arraynamevalue[1]);
                    }
                }
            }
            var _data = JsonHelper.SerializeDictionaryToJsonString(keyValuePairs == null ? null : keyValuePairs);

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
            string htmlText = RenderViewTostring.RenderPartialView(this, "PemsCalculationResultsView", tasks);

            Aspose.Words.Document doc = new Aspose.Words.Document();
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.InsertHtml(htmlText);
            doc.Save(Server.MapPath("../UpLoadFiles/" + taskId + ".doc"));

            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("~/Views/Calculation/PemsCalculationResultsView?taskId=" + taskId);
            ////使用Cookie设置AllowAutoRedirect属性为false,是解决“尝试自动重定向的次数太多。”的核心
            //request.CookieContainer = new CookieContainer();
            //request.AllowAutoRedirect = false;
            //WebResponse response = (WebResponse)request.GetResponse();
            //Stream sm = response.GetResponseStream();
            //System.IO.StreamReader streamReader = new System.IO.StreamReader(sm);
            ////将流转换为字符串
            //string html = streamReader.ReadToEnd();
            //streamReader.Close();
            return Success();
            //SynchronizedPechkin sc = new SynchronizedPechkin(new GlobalConfig()
            //    .SetMargins(new Margins() { Left = 0, Right = 0, Top = 0, Bottom = 0 })); //设置边距
        }

        public ActionResult CalculationView(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult SaveToLocal(string taskid, string taskDate)
        {
            string filePath = Server.MapPath("~") + "/UpLoadModelFiles/" + taskid + ".doc";
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/octet-stream";

            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(taskDate+".doc"));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult UploadExcel(string taskid)
        {
            var file = Request.Files[0];
            byte[] byts = new byte[file.InputStream.Length];
            file.InputStream.Read(byts, 0, byts.Length);
            var requestContent = Encoding.Default.GetString(byts);
            string[] array = requestContent.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int totalCount = array.Length; // 导入的记录总数 
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Trim() != "检验报告基本参数" || array[i].Trim() != "检验结论" || array[i].Trim() != "检验车辆基本参数")
                {
                    string[] arraynamevalue = array[i].Split('=');
                    if (arraynamevalue.Length == 2)
                    {
                        keyValuePairs.Add(arraynamevalue[0], arraynamevalue[1] == "" ? "—" : arraynamevalue[1]);
                    }
                }
            }
            //自定义模板数值
            var _data = JsonHelper.SerializeDictionaryToJsonString(keyValuePairs == null ? null : keyValuePairs);
            LoadResultModel _loadResultModel = new LoadResultModel
            {
                //检验车辆基本参数
                //VIN号=
                //行驶里程(km)=
                //轮胎数量(个)=
                //编号=
                reportNo = keyValuePairs["报告编号"],
                internalNo = keyValuePairs["内部编号"],
                productName = keyValuePairs["产品名称"],
                productTrademark = keyValuePairs["产品商标"],
                productModel = keyValuePairs["产品型号"],
                inspectedUnits = keyValuePairs["受检单位"],
                inspectionCategory = keyValuePairs["检验类别"],
                dateDelivery = keyValuePairs["发送日期"],
                nameInspectionUnit = keyValuePairs["检验单位名称"],
                nameInspectionAddress = keyValuePairs["检验单位地址"],
                nameInspectionPhone = keyValuePairs["检验单位电话"],
                nameInspectionFax = keyValuePairs["检验单位传真"],
                nameInspectionZipCode = keyValuePairs["检验单位邮编"],
                nameInspectionEmail = keyValuePairs["检验单位E_mail"],
                nameTestUnit = keyValuePairs["受检单位名称"],
                nameTestAddress = keyValuePairs["受检单位地址"],
                nameTestPhone = keyValuePairs["受检单位电话"],
                nameTestFax = keyValuePairs["受检单位传真"],
                nameTestZipCode = keyValuePairs["受检单位邮编"],
                nameTestEmail = keyValuePairs["受检单位E_mail"],
                nameTest = keyValuePairs["受检单位名称"],
                sampleName = keyValuePairs["样品名称"],
                modelName = keyValuePairs["型号"],
                tradeMark = keyValuePairs["商标"],
                productionUnit = keyValuePairs["生产单位"],
                dateManufacture = keyValuePairs["生产日期"],
                sampleSender = keyValuePairs["送样者"],
                sampleDeliveryDate = keyValuePairs["送样日期"],
                quantitySamples = keyValuePairs["样品数量"],
                inspectionModel1 = keyValuePairs["检验类别1"],
                dateIssuance = keyValuePairs["签发日期"],
                remark = keyValuePairs["备注"],
                approval = keyValuePairs["批准"],
                examine = keyValuePairs["审核"],
                chiefInspector = keyValuePairs["主检"]
            };
            var data = _readRstService.ReadTestInfo(Server.MapPath(Result_Root_Url), "b88859c7-dee8-4ec0-bcd5-90c9fa6a71e7");//测试
            //var _json = JsonHelper.SerializeDictionaryToJsonString(data);
            var pemsTask = _sysPemsTaskService.GetSysPemsTaskByTaskId("4c668a4b-dcfd-4ac8-94ff-8512e5fb4c0f");
            var taskResult = _sysTaskResultService.GetTaskResultByTaskId("4c668a4b-dcfd-4ac8-94ff-8512e5fb4c0f");
            //System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string alt_avg = "";
            if (data.TryGetValue("Alt_avg", out alt_avg))
            {
                alt_avg = data["Alt_avg"];
            }
            else { alt_avg = ""; }
            string _work_ref = "";
            if (data.TryGetValue("work_ref", out _work_ref))
            {
                _work_ref = data["work_ref"];
            }
            else { _work_ref = ""; }
            CalculationResultData _calculationResultData = new CalculationResultData
            {
                r_payload = data["r_payload"],
                T_air_avg = data["T_air_avg"],
                work_ref = _work_ref == null ? "" : pemsTask.WhtcPower.ToString(),
                Alt_avg = alt_avg == null ? "" : alt_avg,
                work_times = data["work_times"],
                odo_trips = data["odo_trips"],
                p_odo_u = data["p_odo_u"],
                p_odo_r = data["p_odo_r"],
                p_odo_m = data["p_odo_m"],
                p_odo_acc = data["p_odo_acc"],
                p_odo_dec = data["p_odo_dec"],
                p_odo_cru = data["p_odo_cru"],
                p_odo_idle = data["p_odo_idle"],
                p_BSCO_vWDs = data["p_BSCO_vWDs"],
                p_BSNOx_vWDs = data["p_BSNOx_vWDs"],
                p_BSPN_vWDs = data["p_BSPN_vWDs"],
                BSCO_avg = data["BSCO_avg"],
                BSHC_avg = data["BSHC_avg"],
                BSNOx_avg = data["BSNOx_avg"],
                BSPN_avg = data["BSPN_avg"],
                BSCO2_avg = data["BSCO2_avg"],
                NO_wWDs = data["NO_wWDs"],
                NO_valid_wWDs = data["NO_valid_wWDs"],
                p_valid_wWDs = data["p_valid_wWDs"],
                p_pow_f = data["p_pow_f"],
                work_ref2 = _work_ref == null ? "" : pemsTask.WhtcPower.ToString(),
            };
            
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
                WhtcPower = pemsTask.WhtcPower.ToString(),
                calculationResultData = _calculationResultData,
                loadResultModel = _loadResultModel
            };
            string htmlText = RenderViewTostring.RenderPartialView(this, "PemsCalculationResultsViewDown", tasks);

            Aspose.Words.Document doc = new Aspose.Words.Document();
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.InsertHtml(htmlText);
            doc.Save(Server.MapPath("../UpLoadModelFiles/" + taskid + ".doc"));
            //上传结束 保存上传数据信息包括文件路径
            var sysData = new SysData
            {
                UserId = WorkUser.UserId,
                DataName = taskid,
                DataStatus = 1,
                IsPublic = 0,
                IsDel = 0,
                UploadDate = DateTime.Now,
                DataUrl = "../UpLoadModelFiles/" + taskid + ".doc"
            };
            if (_sysDataService.InsertData(sysData) > 0)
            {
                return Success(_data, "");
            }
            return Failure();
        }

        public ActionResult ImportExcelView(string taskid)
        {
            ViewBag.TaskId = taskid;
            //UploadExcelCommon.dt = new DataTable();
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
                            KrNox = calculationModel.Nox,
                            EmissionStandard = calculationModel.EmissionStandard
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