
using Aspose.Words;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
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
            TempData["Result"] = _json;
           // ViewBag.Result = _json;
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

        /// <summary>
        /// 將Html文字 輸出到PDF檔裡
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public byte[] ConvertHtmlTextToPDF(string htmlText)
        {
            if (string.IsNullOrEmpty(htmlText))
            {
                return null;
            }
            //避免當htmlText無任何html tag標籤的純文字時，轉PDF時會掛掉，所以一律加上<p>標籤
            htmlText = "<p>" + htmlText + "</p>";

            MemoryStream outputStream = new MemoryStream();//要把PDF寫到哪個串流
            byte[] data = Encoding.UTF8.GetBytes(htmlText);//字串轉成byte[]
            MemoryStream msInput = new MemoryStream(data);
            iTextSharp.text.Document doc = new iTextSharp.text.Document();//要寫PDF的文件，建構子沒填的話預設直式A4
            PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);
            //指定文件預設開檔時的縮放為100%
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
            //開啟Document文件 
            doc.Open();
            //使用XMLWorkerHelper把Html parse到PDF檔裡
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8);
            //將pdfDest設定的資料寫到PDF檔
            PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
            writer.SetOpenAction(action);
            doc.Close();
            msInput.Close();
            outputStream.Close();
            //回傳PDF檔案 
            return outputStream.ToArray();

        }

        public ActionResult CalculationView(string id)
        {
            ViewBag.Id = id;
            return View();
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
            var data = _readRstService.ReadTestInfo(Server.MapPath(Result_Root_Url), "b88859c7-dee8-4ec0-bcd5-90c9fa6a71e7");//测试
            //var _json = JsonHelper.SerializeDictionaryToJsonString(data);
            var pemsTask = _sysPemsTaskService.GetSysPemsTaskByTaskId("4c668a4b-dcfd-4ac8-94ff-8512e5fb4c0f");
            var taskResult = _sysTaskResultService.GetTaskResultByTaskId("4c668a4b-dcfd-4ac8-94ff-8512e5fb4c0f");
            //System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            
            //$("#r_payload").text(json["r_payload"]);
            //$("#T_air_avg").text(json["T_air_avg"]);
            //$("#work_ref").text(json["work_ref"] == null ? $("#WhtcPower").val() : json["work_ref"]);
            //$("#Alt_avg").text(json["Alt_avg"]);
            //$("#Work_times").text(json["Work_times"]);
            //$("#odo_trips").text(json["odo_trips"]);
            //$("#p_odo_u").text(json["p_odo_u"]);
            //$("#p_odo_r").text(json["p_odo_r"]);
            //$("#p_odo_m").text(json["p_odo_m"]);
            //$("#p_odo_acc").text(json["p_odo_acc"]);
            //$("#p_odo_dec").text(json["p_odo_dec"]);
            //$("#p_odo_cru").text(json["p_odo_cru"]);
            //$("#p_odo_idle").text(json["p_odo_idle"]);
            //$("#p_BSCO_vWDs").text(json["p_BSCO_vWDs"]);
            //$("#p_BSNOx_vWDs").text(json["p_BSNOx_vWDs"]);
            //$("#p_BSPN_vWDs").text(json["p_BSPN_vWDs"]);
            //$("#BSCO_avg").text(json["BSCO_avg"]);
            //$("#BSHC_avg").text(json["BSHC_avg"]);
            //$("#BSNOx_avg").text(json["BSNOx_avg"]);
            //$("#BSPN_avg").text(json["BSPN_avg"]);
            //$("#BSCO2_avg").text(json["BSCO2_avg"]);
            //$("#NO_wWDs").text(json["NO_wWDs"]);
            //$("#NO_valid_wWDs").text(json["NO_valid_wWDs"]);
            //$("#p_valid_wWDs").text(json["p_valid_wWDs"]);
            //$("#p_Pow_f").text(json["p_Pow_f"]);
            //$("#work_ref2").text(json["work_ref"] == null ? $("#WhtcPower").val() : json["work_ref"]);
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
                WhtcPower = pemsTask.WhtcPower.ToString()//,
                //calculationResultData = _calculationResultData
            };
            string htmlText = RenderViewTostring.RenderPartialView(this, "PemsCalculationResultsView", tasks);

            Aspose.Words.Document doc = new Aspose.Words.Document();
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.InsertHtml(htmlText);
            doc.Save(Server.MapPath("../UpLoadFiles/" + taskid + ".doc"));
            return Success(_data, "");
        }

        public ActionResult ImportExcelView(string taskid)
        {
            ViewBag.TaskId = taskid;
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