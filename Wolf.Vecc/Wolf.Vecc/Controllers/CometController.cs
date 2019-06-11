using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Wolf.Vecc.Comm.Helpers;
using Wolf.Vecc.IService.ISysService;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.Controllers
{
    public class CometController : AsyncController
    {
        private readonly ISysTaskResultService _sysTaskResultService;
        private readonly ISysTaskService _sysTaskService;
        private readonly ISysPemsTaskService _sysPemsTaskService;
       
        private readonly string FileUrl = GlobalConfigHelper.GetFileUrl();
        System.Timers.Timer timer;

        public CometController()
        {
        }

        public CometController(ISysPemsTaskService sysPemsTaskService, ISysTaskService sysTaskService, ISysTaskResultService sysTaskResultService)
        {
            _sysTaskResultService = sysTaskResultService;
            _sysPemsTaskService = sysPemsTaskService;
            _sysTaskService = sysTaskService;
        }

        // GET: Comet
        public ActionResult Index()
        {
            return View();
        }

        #region 后台计算
        public void CalculationPollingAsync(string id)
        {
            //校验成功  文件转换成功 计算完成 报告生成成功
            //任务状态，0等待处理，1正在校验，2校验完成，3正在转换文件，4转换文件完成，5正在计算，6计算完成，7生成报完成
            //-21示数据校验失败，-22表示文件转换失败，-23表示计算失败，-24表示报告生成失败，-25系统异常
            timer = new System.Timers.Timer(1000);
            AsyncManager.OutstandingOperations.Increment();
            int taskstatus = 0;
            string statusstr = "";
            timer.Elapsed += (sender, e) =>
            {
                var task = _sysTaskService.GetSysTaskByTaskId(id);
                if (task != null)
                {
                    taskstatus = task.TaskStatus;
                    if (taskstatus == 4)
                    {
                        var pemsTask = _sysPemsTaskService.GetSysPemsTaskByTaskId(task.TaskId.ToString());
                        Dictionary<string, object> dic_result = new Dictionary<string, object>();
                        dic_result.Add("vehicle_model", pemsTask.VehicleModel);
                        dic_result.Add("pems_manufacturer", pemsTask.PemsFactory);
                        dic_result.Add("vehicle_class", pemsTask.VehicleType);
                        dic_result.Add("work_cyc_ref", pemsTask.WhtcPower);
                        dic_result.Add("mass_curb", pemsTask.VehicleQuality);
                        dic_result.Add("trq_ref_max", pemsTask.MaxRefTorque);
                        dic_result.Add("mass_max", pemsTask.MaxQuality);
                        dic_result.Add("power_max", pemsTask.MaxPower);
                        dic_result.Add("r_payload", pemsTask.PercentageLoad);
                        dic_result.Add("inputfile_path", FileUrl + task.TaskId.ToString() + "/OriginalFile/");
                        dic_result.Add("rst_file_dir", FileUrl + task.TaskId.ToString() + "/ResultFiles/");
                        dic_result.Add("task_id", task.TaskId.ToString());
                        dic_result.Add("report_url", "");
                        dic_result.Add("kr_nox", pemsTask.KrNox);

                        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                        string send_data = javaScriptSerializer.Serialize(dic_result);
                        string posturl = GlobalConfigHelper.GetPostUrl();
                        LogHelper.LogInfo(posturl);
                        PubHelp.PostData(posturl, send_data);
                        LogHelper.LogInfo("经过PubHelp.PostData");
                        LogHelper.LogInfo(send_data);
                        _sysTaskService.UpdateSysTaskStatus(task.TaskId, 5);
                        statusstr = "当前状态是：开始计算……";
                    }
                    else
                    {
                        if (taskstatus >= 6)//完成计算
                        {
                            statusstr = "当前状态是：" + PubHelp.GetStatus(taskstatus);
                            //跳转结果页面
                            //RedirectToAction("Calculation", "PemsCalculationResults", new { taskId = task.TaskId });
                            //RedirectToRoute(new { controller = "Calculation", action = "PemsCalculationResults" });
                            //string url = "PemsCalculateNew.aspx?id=" + ViewState["TaskID"].ToString();
                            //Response.Redirect("/Calculation/PemsCalculationResults");
                        }
                        else
                        {
                            if (taskstatus != -23)//显示每个阶段的计算结果
                            {
                                statusstr = "当前状态是：" + PubHelp.GetStatus(taskstatus);
                            }
                            else //如果是-23的话就是计算失败
                            {
                                //模拟数据结果
                                //SysTaskResult sysTaskResult = new SysTaskResult
                                //{
                                //    DetailErrorInfo = "ddd",
                                //    PdfReportUrl = "d",
                                //    ErrorInfo = "dss",
                                //    PlaceTest = "",
                                //    ResultDirUrl = "",
                                //    RouteDescription = "",
                                //    TaskID = task.TaskId,
                                //    TaskResultID = Guid.NewGuid(),
                                //    TestDate = DateTime.Now.ToShortDateString(),
                                //    TaskFinishTime = DateTime.Now,
                                //    TestPerson = "",
                                //    TestTime = DateTime.Now.ToShortDateString()
                                //};

                                //_sysTaskResultService.Insert(sysTaskResult);
                                var taskResult = _sysTaskResultService.GetTaskResultByTaskId(task.TaskId.ToString());
                                if (taskResult != null)
                                {
                                    statusstr = "计算失败，失败原因是：" + taskResult.ErrorInfo;
                                    //删除结果记录 修改task状态为4
                                    //using (TransactionScope transaction = new TransactionScope())
                                    //{
                                    //    if (_sysTaskService.UpdateSysTaskStatus(task.TaskId, 4))
                                    //    {
                                    //        _sysTaskResultService.DeleteByTaskId(task.TaskId.ToString());
                                    //    }
                                    //    transaction.Complete();
                                    //}
                                    timer.Close();
                                }
                            }
                        }
                    }
                    AsyncManager.Parameters["statusstr"] = statusstr;
                    AsyncManager.Parameters["taskstatus"] = taskstatus;
                }
                AsyncManager.OutstandingOperations.Decrement();
            };
            //启动计时器
            timer.Start();
        }

        
        public ActionResult CalculationPollingCompleted(string statusstr, int taskstatus)
        {
            return Json(new
            {
                result = statusstr,
                status = taskstatus
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 数据上传审核实时数量
        #endregion
    }
}