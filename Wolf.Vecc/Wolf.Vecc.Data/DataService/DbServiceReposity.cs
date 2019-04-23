using EntityFramework.Extensions;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Data.DataContext;

namespace Wolf.Vecc.Data.DataService
{
    public class DbServiceReposity : IDbServiceReposity
    {
        private DbContext _dbContext = DbContextFactory.CallDbContext();//线程内唯一
        public DbServiceReposity(DbContext __dbContext)
        {
            _dbContext = __dbContext;
        }

        public int Add<T>(T t) where T : class
        {
            PropertyInfo createDate = typeof(T).GetProperty("CreateDate");
            PropertyInfo approvalDate = typeof(T).GetProperty("ApprovalDate");
            PropertyInfo uploadDate = typeof(T).GetProperty("UploadDate");
            PropertyInfo isDeleted = typeof(T).GetProperty("IsDel");
            //默认数据
            if (createDate != null)
            {
                createDate.SetValue(t, DateTime.Now);
            }
            if (approvalDate != null)
            {
                approvalDate.SetValue(t, DateTime.Now);
            }
            if (uploadDate != null)
            {
                uploadDate.SetValue(t, DateTime.Now);
            }
            if (isDeleted != null)
            {
                isDeleted.SetValue(t, 0);//添加时默认设置为未删除
            }
            _dbContext.Set<T>().Add(t);
            return SaveChanges();
        }

        public IQueryable<T> All<T>(params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return includeProperties.Aggregate<Expression<Func<T, object>>,
               IQueryable<T>>(_dbContext.Set<T>(),
               (set, property) => set.Include(property));
        }

        public int Count<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return _dbContext.Set<T>().Where(predicate).Count();
        }

        public int Count(string sql, params object[] paramseter)
        {
            return Convert.ToInt32(_dbContext.Database.SqlQuery<decimal>(sql, paramseter).ToList()[0]);
        }

        public int Delete<T>(T t) where T : class
        {
            _dbContext.Entry<T>(t).State = EntityState.Deleted;
            return SaveChanges();
        }

        public int Delete(string sql, params object[] paramseter)
        {
            return _dbContext.Database.ExecuteSqlCommand(sql, paramseter);
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        public T Find<T>(object id) where T : class
        {
            return _dbContext.Set<T>().Find(id);
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> whereLamdaba) where T : class
        {
            return _dbContext.Set<T>().Where(whereLamdaba).FirstOrDefault();
        }

        public List<T> Query<T>(string sql, params object[] paramenters) where T : class
        {
            return _dbContext.Database.SqlQuery<T>(sql, paramenters).ToList();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public List<T> SelectPageList<T>(int pageIndex, int pagesize, List<T> list)
        {
            var query = from oneItem in list select oneItem;
            return query.Take(pagesize * pageIndex).Skip(pagesize * (pageIndex - 1)).ToList();
        }

        public int DeleteByExpression<T>(Expression<Func<T, bool>> predicateWhere) where T : class
        {
            //如果为空将获取整张表数据并删掉整张表
            //var source = predicateWhere == null ? _dbContext.Set<T>() : _dbContext.Set<T>().Where(predicateWhere);
            //如果条件为空返回null数据集合避免删除整张表
            int _num = 0;
            var source = predicateWhere == null ? null : _dbContext.Set<T>().Where(predicateWhere);
            if (source != null)
            {
                _num = source.Delete();
            }
            return _num;
        }

        public int Sum<T>(Expression<Func<T, bool>> predicateWhere, Expression<Func<T, int>> predicateSelect) where T : class
        {
            return _dbContext.Set<T>().Where(predicateWhere).Select(predicateSelect).Sum();
        }

        public int Update<T>(T t) where T : class
        {
            _dbContext.Entry<T>(t).State = EntityState.Modified;
            return SaveChanges();
        }

        public int Update(string sql, params object[] paramseter)
        {
            return _dbContext.Database.ExecuteSqlCommand(sql, paramseter);
        }

        public IQueryable<T> Where<T>(Expression<Func<T, bool>> whereLambada) where T : class
        {
            return _dbContext.Set<T>().Where(whereLambada);
        }


        public IEnumerable<T> GetWhereSearch<T>(Expression<Func<T, bool>> where) where T : class
        {
            if (where == null)
                return _dbContext.Set<T>().AsExpandable();//.AsExpandable();//这个最重要.否则会出如题的错误.
            return _dbContext.Set<T>().AsExpandable().Where(where);
        }

        public IEnumerable<T> Where<T>(string sql, params object[] paramseters) where T : class
        {
            return _dbContext.Database.SqlQuery<T>(sql, paramseters);
        }
    }
}
