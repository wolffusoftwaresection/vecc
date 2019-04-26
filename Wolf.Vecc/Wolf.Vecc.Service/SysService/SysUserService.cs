using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Core.LambdaExt;
using Wolf.Vecc.Data.DataService;
using Wolf.Vecc.IService.ISysService;
using Wolf.Vecc.Model.SysModel;
using Wolf.Vecc.Model.ViewModel;

namespace Wolf.Vecc.Service.SysService
{
    public class SysUserService : ISysUserService
    {
        private readonly IDbServiceReposity _dbServiceReposity;
        public SysUserService(IDbServiceReposity dbServiceReposity)
        {
            _dbServiceReposity = dbServiceReposity;
        }

        public List<SysUsers> All()
        {
            return _dbServiceReposity.All<SysUsers>().Where(d => d.IsDel == 0).ToList();
        }

        public int GetApprovalNumber()
        {
            return _dbServiceReposity.Where<SysUsers>(d => d.IsDel == 0 && d.AccountStatus == 1).Count();
        }

        public List<int> GetRoleIdByUserId(int uId)
        {
            return _dbServiceReposity.Where<SysUsers>(d => d.IsDel == 0 && d.Id == uId).Select(d=>d.RoleId).ToList();
        }

        public SysUsers GetUserById(int Id)
        {
            return _dbServiceReposity.FirstOrDefault<SysUsers>(d => d.Id == Id);
        }

        public SysUsers GetUserByUserName(string userName)
        {
            return _dbServiceReposity.FirstOrDefault<SysUsers>(d => d.UserName == userName.Trim() && d.IsDel == 0);
        }

        public List<SysUsers> GetUserList(UserApprovalViewModel userApprovalViewModel)
        {
            Expression<Func<SysUsers, bool>> searchPredicate = PredicateExtensions.True<SysUsers>();
            searchPredicate = searchPredicate.And(d => d.IsDel == 0 && d.UserType > 0);
            if (userApprovalViewModel.UserName != null)
            {
                searchPredicate = searchPredicate.And(d => d.UserName.Contains(userApprovalViewModel.UserName));
            }
            if (userApprovalViewModel.State > 0)
            {
                searchPredicate = searchPredicate.And(d => d.AccountStatus == userApprovalViewModel.State);
            }
            return _dbServiceReposity.GetWhereSearch(searchPredicate).ToList();
        }

        public int Insert(SysUsers sysUser)
        {
            return _dbServiceReposity.Add(sysUser);
        }

        public int Update(SysUsers sysUser)
        {
            return _dbServiceReposity.Update(sysUser);
        }

        /// <summary>
        /// 如果为true表示不存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool UserRepeat(string userName)
        {
            return _dbServiceReposity.FirstOrDefault<SysUsers>(d => d.UserName == userName) == null;
        }
    }
}
