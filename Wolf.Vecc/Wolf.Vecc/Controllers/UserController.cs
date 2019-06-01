using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using Wolf.Vecc.Comm.Helpers;
using Wolf.Vecc.IService.ISysService;
using Wolf.Vecc.Model.ResultModel;
using Wolf.Vecc.Model.ViewModel;

namespace Wolf.Vecc.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        // GET: User
        private readonly ISysUserService _sysUserService;
        private readonly ISysDataService _sysDataService;
        private readonly ISysApprovalDataService _sysApprovalDataService; 

        public UserController()
        {
             
        }

        public UserController(ISysUserService sysUserService, ISysDataService sysDataService, ISysApprovalDataService sysApprovalDataService)
        {
            _sysUserService = sysUserService;
            _sysDataService = sysDataService;
            _sysApprovalDataService = sysApprovalDataService;
        }

        public ActionResult UpdatePwModel(UpdatePwModel updatePwModel)
        {
            //验证旧密码
            var user = _sysUserService.GetUserById(WorkUser.UserId);
            if (UtilityHelper.CreateHashCodePW(updatePwModel.OldPassword, user.Salt) == user.Password)
            {
                //更新新密码
                user.Password = UtilityHelper.CreateHashCodePW(updatePwModel.NewPassword, user.Salt);
                if (_sysUserService.Update(user) > 0)
                {
                    return Success("重置完成!");
                }
            }
            else
            {
                return Failure("密码错误!");
            }
            return Failure("重置失败!");
        }

        /// <summary>
        /// 用户个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult ProfileView()
        {
            var user = _sysUserService.GetUserById(WorkUser.UserId);
            ViewBag.userType = VeccModelHelp.GetUserType(user.UserType);
            return View(user);
        }

        /// <summary>
        /// 用户查询上传数据审核结果页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UserFindingsAuditView()
        {
            return View();
        }

        public JsonResult UserFindingsAuditList(FindingsAuditViewModel findingsAuditViewModel)
        {
            var dataList = _sysDataService.GetSysDataList(findingsAuditViewModel);
            var list = from r in dataList
                       join cr in _sysApprovalDataService.All()
                       on r.Id equals cr.DataId into tt
                       from crr in tt.DefaultIfEmpty()
                       select new UserFindingsAuditModel
                       {
                           DataName = r.DataName,
                           DataStatus = r.DataStatus,
                           DataUrl = r.DataUrl,
                           Id = r.Id,
                           UploadDate = r.UploadDate,
                           ApprovalDate = crr == null ? "" : crr.ApprovalDate.ToString("yyyy-MM-dd hh:mm:ss"),
                           ApprovalRemark = crr == null ? "":crr.ApprovalRemark
                       };

            var count = list.Count();
            var modelPage = list.OrderByDescending(o => o.Id)
                .ToPagedList(findingsAuditViewModel.PageIndex, findingsAuditViewModel.PageSize);
            return Json(new { total = count, rows = modelPage }, JsonRequestBehavior.AllowGet);
        }
    }
}