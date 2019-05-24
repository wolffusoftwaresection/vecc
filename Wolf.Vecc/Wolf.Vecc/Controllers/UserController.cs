using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolf.Vecc.Comm.Helpers;
using Wolf.Vecc.IService.ISysService;
using Wolf.Vecc.Model.ViewModel;

namespace Wolf.Vecc.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        // GET: User
        private readonly ISysUserService _sysUserService;

        public UserController()
        {
             
        }

        public UserController(ISysUserService sysUserService)
        {
            _sysUserService = sysUserService;
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
    }
}