using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Wolf.Vecc.Comm.Helpers;
using Wolf.Vecc.IService.ISysService;

namespace Wolf.Vecc.Controllers
{
    public class CometController : AsyncController
    {
        private readonly ISysTaskResultService _sysTaskResultService;
        private readonly ISysTaskService _sysTaskService;
        private readonly ISysPemsTaskService _sysPemsTaskService;
        private readonly ISysUserService _sysUserService;
        private readonly string FileUrl = GlobalConfigHelper.GetFileUrl();

        public CometController(ISysPemsTaskService sysPemsTaskService, ISysTaskService sysTaskService, ISysUserService sysUserService, ISysTaskResultService sysTaskResultService)
        {
            _sysUserService = sysUserService;
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
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            AsyncManager.OutstandingOperations.Increment();
            int taskstatus = 0;
            string statusstr = "";
            timer.Elapsed += (sender, e) =>
            {
                var task = _sysTaskService.GetSysTaskByTaskId(id);
                if (task != null)//开始计算
                {
                    taskstatus = task.TaskStatus;
                    if (taskstatus == 4)
                    {
                        var pemsTask = _sysPemsTaskService.GetSysPemsTaskByTaskId(id);
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
                        dic_result.Add("inputfile_path", FileUrl + id + "/OriginalFile/");
                        dic_result.Add("rst_file_dir", FileUrl + id + "/ResultFiles/");
                        dic_result.Add("task_id", id);
                        dic_result.Add("report_url", "");
                        dic_result.Add("kr_nox", pemsTask.KrNox);

                        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                        string send_data = javaScriptSerializer.Serialize(dic_result);
                        string posturl = GlobalConfigHelper.GetPostUrl();
                        PubHelp.PostData(posturl, send_data);
                        _sysTaskService.UpdateSysTaskStatus(new Guid(id), 5);
                        statusstr = "当前状态是：开始计算……";
                    }
                    else
                    {
                        if (taskstatus >= 6)//完成计算
                        {
                            timer.Stop();
                            //跳转结果页面
                            RedirectToAction("Calculation", "PemsCalculationResults", new { taskId = id });
                            //string url = "PemsCalculateNew.aspx?id=" + ViewState["TaskID"].ToString();
                            //Response.Redirect(url);
                        }
                        else
                        {
                            if (taskstatus != -23)
                            {
                                statusstr = "当前状态是：" + PubHelp.GetStatus(taskstatus);
                            }
                            else
                            {
                                var taskResult = _sysTaskResultService.GetTaskResultByTaskId(id);
                                if (taskResult != null)
                                {
                                    statusstr = "计算失败，失败原因是：" + taskResult.ErrorInfo;
                                }
                            }
                        }
                    }
                    AsyncManager.Parameters["status"] = statusstr;
                    AsyncManager.OutstandingOperations.Decrement();
                    //启动计时器
                    timer.Start();
                }
            };
        }

        
        public ActionResult CalculationPollingCompleted(string status)
        {
            return Json(new
            {
                d = status
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 新用户注册实时数量
        public void ApprovalPollingAsync()
        {
            System.Timers.Timer timer = new System.Timers.Timer(5000);
            AsyncManager.OutstandingOperations.Increment();
            timer.Elapsed += (sender, e) =>
            {
                AsyncManager.Parameters["now"] = _sysUserService.GetApprovalNumber().ToString();
                AsyncManager.OutstandingOperations.Decrement();
            };
            //启动计时器
            timer.Start();
        }

        public ActionResult ApprovalPollingCompleted(string now)
        {
            return Json(new
            {
                d = now
            },JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 数据上传审核实时数量
        #endregion
    }
}