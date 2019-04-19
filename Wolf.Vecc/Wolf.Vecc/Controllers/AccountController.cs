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
using Wolf.Vecc.Model.ViewModel;

namespace Wolf.Vecc.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
                        //保存Cookie
                        VeccFormsAuthentication<VeccUserDataPrincipal>.SetAuthCookie(user.UserName, userData, user.View_RememberFlag);
                        //return RedirectToAction("Index", "Index");
                        return Success("");
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

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Login");
        }
    }
}