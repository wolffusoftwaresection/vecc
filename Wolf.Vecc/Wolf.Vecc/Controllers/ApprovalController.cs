﻿using System;
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
        private readonly ISysDataService _dataService;
        private readonly ISysApprovalUserService _approvalUserService;
        private readonly ISysApprovalDataService _approvalDataService;
        public ApprovalController(ISysUserService userService, ISysApprovalUserService approvalUserService, ISysDataService dataService, ISysApprovalDataService sysApprovalDataService)
        {
            _userService = userService;
            _approvalUserService = approvalUserService;
            _approvalDataService = sysApprovalDataService;
            _dataService = dataService;
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

        public JsonResult DataApprovalList(DataApprovalViewModel dataApprovalViewModel)
        {
            var userList = _dataService.GetSysDataList(dataApprovalViewModel);

            var list = from l in userList
                       join u in _userService.All()
                       on l.UserId equals u.Id
                       select new 
                       {
                           l.Id,
                           l.IsPublic,
                           l.UploadDate,
                           l.DataName,
                           l.DataStatus,
                           l.DataUrl,
                           u.UserName
                       };

            var count = list.Count();
            var modelPage = list.OrderByDescending(o => o.Id)
                .ToPagedList(dataApprovalViewModel.PageIndex, dataApprovalViewModel.PageSize);
            return Json(new { total = count, rows = modelPage }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户同意
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
        /// 数据同意
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgreeData(int Id)
        {
            //更新用户状态为通过审批并记录审批信息表
            SysApprovaData sysApprovaData = new SysApprovaData
            {
                ApprovalDate = DateTime.Now,
                IsDel = 0,
                AccountStatus = 3,
                ApprovalRemark = "",
                DataId = Id,
                VeccUserId = WorkUser.UserId
            };
            var result = false;
            using (TransactionScope transaction = new TransactionScope())
            {
                var data = _dataService.GetDataById(Id);
                if (data != null)
                {
                    data.DataStatus = 3;
                    if (_dataService.Update(data) > 0)
                    {
                        result = _approvalDataService.Insert(sysApprovaData) > 0;
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

        public ActionResult RefuseData(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        public JsonResult ChangePublic(int Id, int Pub)
        {
            var data = _dataService.GetDataById(Id);
            data.IsPublic = Pub == 0 ? 1 : 0;
            if (_dataService.Update(data) > 0)
            {
                return Success();
            }
            return Failure();
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

        public JsonResult DoRefuseData(int Id, string remark)
        {
            //更新用户状态为通过审批并记录审批信息表
            SysApprovaData sysApprovaData = new SysApprovaData
            {
                ApprovalDate = DateTime.Now,
                IsDel = 0,
                AccountStatus = 2,
                ApprovalRemark = remark,
                DataId = Id,
                VeccUserId = WorkUser.UserId
            };
            var result = false;
            using (TransactionScope transaction = new TransactionScope())
            {
                var data = _dataService.GetDataById(Id);
                if (data != null)
                {
                    data.DataStatus = 2;
                    if (_dataService.Update(data) > 0)
                    {
                        result = _approvalDataService.Insert(sysApprovaData) > 0;
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