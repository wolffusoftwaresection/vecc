using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Data.DataService
{
    public interface IDbServiceReposity
    {
        #region 单条操作
        int Add<T>(T t) where T : class;

        int Update<T>(T t) where T : class;

        int Delete<T>(T t) where T : class;
        #endregion

        #region where条件操作
        IQueryable<T> Where<T>(Expression<Func<T, bool>> whereLambada) where T : class;

        T FirstOrDefault<T>(Expression<Func<T, bool>> whereLamdaba) where T : class;

        int Count<T>(Expression<Func<T, bool>> predicate) where T : class;

        int Sum<T>(Expression<Func<T, bool>> predicateWhere, Expression<Func<T, int>> predicateSelect) where T : class;

        T Find<T>(object id) where T : class;

        IQueryable<T> All<T>(params Expression<Func<T, object>>[] includeProperties) where T : class;

        /// <summary>
        /// 对象Delete方法只能根据对象id删除;该方法可先以多条件查询后得到的集合后删除;不需要对象参数;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateExpression"></param>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        int DeleteByExpression<T>(Expression<Func<T, bool>> predicateWhere) where T : class;
        #endregion

        #region T_SQL数据操作
        IEnumerable<T> Where<T>(string sql, params object[] paramseters) where T : class;

        int Delete(string sql, params object[] paramseter);

        int Update(string sql, params object[] paramseter);

        int Count(string sql, params object[] paramseter);

        /// <summary>
        /// 执行SQL并自传SqlParameter[]参数用于update,insert,deleteSQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        List<T> Query<T>(string sql, params object[] paramenters) where T : class;
        #endregion

        #region Linq分页千万级数据时考虑业务优化过期数据的过滤使linq分页或存储过程分页能适用
        /// <summary>
        /// Linq分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pagesize"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        List<T> SelectPageList<T>(int pageIndex, int pagesize, List<T> list);
        #endregion

        int SaveChanges();
    }
}
