using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Wolf.Vecc.Comm.EnumExtent.ParamsEnum;
using Wolf.Vecc.Comm.Helpers;
using Wolf.Vecc.Data.AuthCore.AuthExt;
using Wolf.Vecc.Data.AuthCore.PrincipalExt;
using Wolf.Vecc.IService.ISysService;
using Wolf.Vecc.Model.SysModel;
using Wolf.Vecc.Model.ViewModel;

namespace Wolf.Vecc.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly ISysUserService _sysUserService;
        public AccountController(IAccountService accountService, ISysUserService sysUserService)
        {
            _accountService = accountService;
            _sysUserService = sysUserService;
        }
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(UserViewModel user)
        {
            var veccUser = _accountService.GetUserByUserName(user.UserName);
            if (veccUser != null)//存在用户
            {
                //判断密码
                if (UtilityHelper.CreateHashCodePW(user.Password, veccUser.Salt) == veccUser.Password)
                {
                    //用户是否已被审批通过方可登陆
                    if (veccUser.AccountStatus == EnumExt.ToInt(UserAccountStatusEnum.PASSED))
                    {
                        var userData = new VeccUserDataPrincipal
                        {
                            UserId = veccUser.Id,
                            UserName = veccUser.UserName,
                            AccountStatus = veccUser.AccountStatus,
                            EnterpriseName = veccUser.EnterpriseName,
                            Phone = veccUser.Phone,
                            RoleId = veccUser.RoleId,
                            UserType = veccUser.UserType
                        };
                        user.View_RememberFlag = true;
                        var lauOutUser = new LayOutUserModel
                        {
                            //ApprovalNumber = _sysUserService.GetApprovalNumber(),
                            UserEmail = veccUser.Email,
                            UserName = veccUser.UserName,
                            UserRole = veccUser.RoleId
                        };
                        //保存Cookie
                        VeccFormsAuthentication<VeccUserDataPrincipal>.SetAuthCookie(user.UserName, userData, user.View_RememberFlag);
                        //return RedirectToAction("Index", "Index");
                        return Success(lauOutUser, "");
                    }
                    else
                    {
                        if (veccUser.AccountStatus == EnumExt.ToInt(UserAccountStatusEnum.PENDING))
                        {
                            return Failure("当前用户状态为:审批中");
                        }
                        else
                        {
                            return Failure("当前用户审核未通过");
                        }
                    }
                }
                else
                {
                    return Failure("用户名或密码错误");
                }
            }
            return Failure("用户不存在");
        }

        /// <summary>
        /// 工程师注册
        /// </summary>
        /// <returns></returns>
        //public ActionResult Register()
        //{
        //    return View();
        //}

        public ActionResult RegisterEngineer()
        {
            return View();
        }

        /// <summary>
        /// 注册时用户是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public JsonResult UserRepeat(string userName)
        {
            if (_accountService.UserRepeat(userName))
            {
                return Json(new { valid = true });
            }
            return Json(new { valid = false });
        }

        /// <summary>
        /// 注册用户信息
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        public ActionResult RegisterModel(RegisterViewModel registerModel)
        {
            //判断验证码

            string salt;
            var code = UtilityHelper.CreateHashCodePW(registerModel.Password, out salt);
            SysUsers sysUsers = new SysUsers
            {
                UserName = registerModel.UserName,
                UserType = registerModel.UserType,
                Email = registerModel.Email,
                EnterpriseName = registerModel.EnterpriseName,
                Password = code,
                Phone = registerModel.Phone,
                RoleId = 3,
                Salt = salt,
                AccountStatus = 1,
                Country = registerModel.Country,
                CreateDate = DateTime.Now
            };
            if (_accountService.InsertSysUser(sysUsers) > 0)
            {
                return Success("注册成功");
            }
            return Failure("注册失败");
        }

        public ActionResult VeccAddUser(RegisterViewModel registerModel)
        {
            string salt;
            var code = UtilityHelper.CreateHashCodePW(registerModel.Password, out salt);
            SysUsers sysUsers = new SysUsers
            {
                UserName = registerModel.UserName,
                UserType = registerModel.UserType,
                Email = registerModel.Email,
                EnterpriseName = registerModel.EnterpriseName,
                Password = code,
                Phone = registerModel.Phone,
                RoleId = 2,
                Salt = salt,
                AccountStatus = 3,//vecc后台添加的检测机构账号默认通过验证
                Country = registerModel.Country,
                CreateDate = DateTime.Now
            };
            if (_accountService.InsertSysUser(sysUsers) > 0)
            {
                return Success("注册成功");
            }
            return Failure("注册失败");
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            //FormsAuthentication.SignOut();
            //return Redirect("~//Login");
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdatePw()
        {
            return View();
        }

        public ActionResult LoginOutView()
        {
            SignOut();
            return RedirectToAction("Login");
        }

        //public ActionResult TestRegister()
        //{
        //    return View();
        //}
    }
}