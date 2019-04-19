using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Wolf.Vecc.Comm.ExceptionExtent;
using Wolf.Vecc.Comm.NlogExtent;
using Wolf.Vecc.Core.AutofacManager;
using Wolf.Vecc.Data.AuthCore;
using Wolf.Vecc.Data.AuthCore.PrincipalExt;
using Wolf.Vecc.Data.DataService;

namespace Wolf.Vecc.Controllers
{
    public class BaseController : Controller
    {
        protected string _errorMsg = string.Empty;
        public readonly IDbServiceReposity _dbServiceReposity = ContainerManager.Resolve<IDbServiceReposity>();
        public BaseController() { }

        #region 返回Json数据
        /// <summary>
        /// </summary>
        /// <param name="_msg"></param>
        /// <returns></returns>
        public JsonResult Success(string _msg = "操作成功")
        {
            return Json(new { Success = true, Msg = string.IsNullOrEmpty(_errorMsg) ? _msg : _errorMsg });
        }
        public JsonResult Failure(string _msg = "操作失败")
        {
            return Json(new { Success = false, Msg = string.IsNullOrEmpty(_errorMsg) ? _msg : _errorMsg });
        }
        #endregion

        protected virtual WorkUser WorkUser
        {
            get
            {
                var user = HttpContext.User as VeccFormsPrincipal<VeccUserDataPrincipal>;
                return SetWorkUser(user);
            }
        }

        private WorkUser SetWorkUser(VeccFormsPrincipal<VeccUserDataPrincipal> veccFormsPrincipal)
        {
            return new WorkUser
            {
                AaccountStatus = veccFormsPrincipal.UserData.AccountStatus,
                UserId = veccFormsPrincipal.UserData.UserId,
                UserName = veccFormsPrincipal.UserData.UserName,
                RoleId = veccFormsPrincipal.UserData.RoleId
            };
        }
    }
}