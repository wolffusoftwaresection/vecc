using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using Wolf.Vecc.Comm.Helpers;
using Wolf.Vecc.Data.AuthCore;
using Wolf.Vecc.IService.ISysService;
using Wolf.Vecc.Model.SysModel;
using Wolf.Vecc.Model.ViewModel;

namespace Wolf.Vecc.Controllers
{
    [VeccAuthorize(Roles = "admin")]
    public class ApprovalController : BaseController
    {
        private readonly ISysUserService _userService;
        private readonly ISysApprovalUserService _approvalUserService;
        public ApprovalController(ISysUserService userService, ISysApprovalUserService approvalUserService)
        {
            _userService = userService;
            _approvalUserService = approvalUserService;
        }

        // GET: Approval
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult UserApprovalList(UserApprovalViewModel userApprovalViewModel)
        {
            var userList = _userService.GetUserList(userApprovalViewModel);
            var count = userList.Count;
            var modelPage = userList.OrderByDescending(o => o.Id)
                .ToPagedList(userApprovalViewModel.PageIndex, userApprovalViewModel.PageSize);
            return Json(new { total = count, rows = modelPage }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 同意
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgreeUser(int Id)
        {
            //更新用户状态为通过审批并记录审批信息表
            SysApprovaUser sysApprovaUser = new SysApprovaUser
            {
                ApprovalDate = DateTime.Now,
                IsDel = 0,
                AccountStatus = 3,
                ApprovalRemark = "",
                UserId = Id,
                VeccUserId = WorkUser.UserId
            };
            var result = false;
            using (TransactionScope transaction = new TransactionScope())
            {
                var user = _userService.GetUserById(Id);
                if (user != null)
                {
                    user.AccountStatus = 3;
                    if (_userService.Update(user) > 0)
                    {
                        result = _approvalUserService.Insert(sysApprovaUser) > 0;
                    }
                }
                transaction.Complete();
            }
            if (result)
            {
                return Success();
            }
            return Failure();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteUser(int Id)
        {
            var user = _userService.GetUserById(Id);
            if (user != null)
            {
                user.IsDel = 1;
                if (_userService.Update(user) > 0)
                {
                    return Success();
                }
            }
            return Failure();
        }

        /// <summary>
        /// 拒绝
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult RefuseUser(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        public JsonResult DoRefuseUser(int Id, string remark)
        {
            //更新用户状态为通过审批并记录审批信息表
            SysApprovaUser sysApprovaUser = new SysApprovaUser
            {
                ApprovalDate = DateTime.Now,
                IsDel = 0,
                AccountStatus = 2,
                ApprovalRemark = remark,
                UserId = Id,
                VeccUserId = WorkUser.UserId
            };
            var result = false;
            using (TransactionScope transaction = new TransactionScope())
            {
                var user = _userService.GetUserById(Id);
                if (user != null)
                {
                    user.AccountStatus = 2;
                    if (_userService.Update(user) > 0)
                    {
                        result = _approvalUserService.Insert(sysApprovaUser) > 0;
                    }
                }
                transaction.Complete();
            }
            if (result)
            {
                return Success();
            }
            return Failure();
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult UserInFo(int Id)
        {
            var user = _userService.GetUserById(Id);
            ViewBag.userType = VeccModelHelp.GetUserType(user.UserType);
            ViewBag.accountStatus = VeccModelHelp.GetAccountStatus(user.AccountStatus);
            return View(user);
        }

        public ActionResult AddUser()
        {
            return View();
        }
    }
}