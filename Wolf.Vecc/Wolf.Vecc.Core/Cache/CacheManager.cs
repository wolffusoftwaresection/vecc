using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Core.Context;
using System.Runtime.Caching;
using System.Data.SqlClient;
using Wolf.Vecc.Comm.EnumExtent.CacheEnum;

namespace Wolf.Vecc.Core.Cache
{
    public class CacheManager: ConnectionString, ICacheManager
    {
        //绝对过期时间（分钟）
        private readonly int cacheTime = 60;
        //在范围内没有操作的相对过期时间（分钟）
        private readonly int slidingExpiration = 60;
        private ObjectCache Cache => MemoryCache.Default;

        /// <summary>
        /// 获取对象类型缓存
        /// </summary>
        /// <typeparam name="T"></Class1.cstypeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(CacheKeyEnum key)
        {
            return (T)Cache[key.Enum2String()];
        }

        /// <summary>
        /// 获取List类型缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> GetList<T>(CacheKeyEnum key)
        {
            return Cache[key.Enum2String()] as List<T>;
        }

        /// <summary>
        /// 缓存辅助必要参数
        /// </summary>
        /// <param name="cacheTime">绝对时间设置该值后无论是否访问过了该时间即失效</param>`
        /// <param name="slidingExpiration">设置该值后在该时间段内有访问可继续有效(超出该时间段内没有访问即失效)</param>
        /// <returns></returns>
        private CacheItemPolicy SetPolicy(Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null)
        {
            var policy = new CacheItemPolicy();
            if (cacheTime.HasValue) { policy.AbsoluteExpiration = DateTime.Now.AddSeconds(cacheTime.Value); }
            if (slidingExpiration.HasValue) { policy.SlidingExpiration = TimeSpan.FromSeconds(slidingExpiration.Value); }
            if (funR != null)
            {
                //该删除时回调方法在手动调用Remove方法时或者绝对相对时间到期时都将触发.
                policy.RemovedCallback = new CacheEntryRemovedCallback(funR);
            }
            if (funU != null)
            {
                //该更新时回调方法只在设置的绝对或相对时间到期时将自动触发。手动调用Remove方法不会触发.
                policy.UpdateCallback = new CacheEntryUpdateCallback(funU);
            }
            return policy;
        }

        /// <summary>
        /// 缓存辅助必要参数(绝对相对时间为分钟)
        /// </summary>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        private CacheItemPolicy SetPolicy(int? cacheTime = null, int? slidingExpiration = null)
        {
            var policy = new CacheItemPolicy();
            if (cacheTime.HasValue) { policy.AbsoluteExpiration = DateTime.Now.AddMinutes(cacheTime.Value); }
            if (slidingExpiration.HasValue) { policy.SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration.Value); }
            return policy;
        }

        /// <summary>
        /// 设置缓存数据为List形式并指定绝对或相对过期时间单位为分钟
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        public void Set<T>(CacheKeyEnum key, List<T> value, int? cacheTime = null, int? slidingExpiration = null)
        {
            if (value == null) { return; }
            Cache.Set(new CacheItem(key.Enum2String(), value), SetPolicy(cacheTime, slidingExpiration));
        }

        /// <summary>
        /// 设置缓存数据为List形式提供删除或更新缓存时的回调方法(2者不可同时实现其中一种回调需为空),
        /// 并指定绝对或相对过期时间单位为秒;使用该方法需要实现接口ICacheCallBack并重写其中的方法;
        /// 在类中使用该方法需要继承抽象类CacheCallBack并实现其中的方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="funR"></param>
        /// <param name="funU"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        public void Set<T>(CacheKeyEnum key, List<T> value, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null)
        {
            if (value == null) { return; }
            Cache.Set(new CacheItem(key.Enum2String(), value), SetPolicy(funR, funU, cacheTime, slidingExpiration));
        }

        /// <summary>
        /// 是否存在指定缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsSet(CacheKeyEnum key)
        {
            return (Cache.Contains(key.Enum2String()));
        }

        /// <summary>
        /// 删除指定缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(CacheKeyEnum key)
        {
            Cache.Remove(key.Enum2String());
        }

        #region 清除所有缓存
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        /// <param name="key"></param>
        private void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void Clear()
        {
            foreach (var item in Cache)
            {
                Remove(item.Key);
            }
        }
        #endregion

        /// <summary>
        /// 设置缓存数据为方法参数形式,并指定绝对或相对过期时间单位为分钟
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="funcT"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        public void Set<T>(CacheKeyEnum key, Func<T> funcT, int? cacheTime = null, int? slidingExpiration = null)
        {
            var value = funcT();
            if (value == null) { return; }

            Cache.Set(new CacheItem(key.Enum2String(), value), SetPolicy(cacheTime, slidingExpiration));
        }

        /// <summary>
        /// 设置缓存数据为方法参数形式,提供删除或更新缓存时的回调方法(2者不可同时实现其中一种回调需为空),
        /// 并指定绝对或相对过期时间单位为秒;使用该方法需要实现接口ICacheCallBack并重写其中的方法;
        /// 在类中使用该方法需要继承抽象类CacheCallBack并实现其中的方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="funcT"></param>
        /// <param name="funR"></param>
        /// <param name="funU"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        public void Set<T>(CacheKeyEnum key, Func<T> funcT, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null)
        {
            var value = funcT();
            if (value == null) { return; }

            Cache.Set(new CacheItem(key.Enum2String(), value), SetPolicy(funR, funU, cacheTime, slidingExpiration));
        }

        /// <summary>
        /// 获取缓存数据请使用该方法常用方法如果缓存过期为null可传入重新获取数据方法并设置缓存数据
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public T Get<T>(CacheKeyEnum key, Func<T> func, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null)
        {
            var data = (T)Cache[key.Enum2String()];
            if (data == null)
            {
                data = func();
                Cache.Set(new CacheItem(key.Enum2String(), data), SetPolicy(funR, funU, cacheTime, slidingExpiration));
            }
            return data;
        }

        /// <summary>
        /// 获取缓存数据请使用该方法常用方法如果缓存过期为null可传入重新获取数据方法并设置缓存数据
        /// </summary>
        /// <typeparam name="T">返回List类型</typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="funR"></param>
        /// <param name="funU"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        public List<T> GetList<T>(CacheKeyEnum key, Func<List<T>> func, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null)
        {
            var data = Cache[key.Enum2String()] as List<T>;
            if (data == null)
            {
                data = func();
                Cache.Set(new CacheItem(key.Enum2String(), data), SetPolicy(funR, funU, cacheTime, slidingExpiration));
            }
            return data;
        }

        /// <summary>
        /// 最新方法如果缓存过时为空将使用传入的方法获取需要设置的最新数据进行缓存
        /// 该方法将返回对象数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        public T Get<T>(CacheKeyEnum key, Func<T> func)
        {
            var data = (T)Cache[key.Enum2String()];
            if (data == null)
            {
                data = func();
                Cache.Set(new CacheItem(key.Enum2String(), data), SetPolicy(cacheTime, slidingExpiration));
            }
            return data;
        }

        /// <summary>
        /// 最新方法如果缓存过时为空将使用传入的方法获取需要设置的最新数据进行缓存
        /// 该方法将返回List数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        public List<T> Get<T>(CacheKeyEnum key, Func<List<T>> func)
        {
            var data = Cache[key.Enum2String()] as List<T>;
            if (data == null)
            {
                data = func();
                Cache.Set(new CacheItem(key.Enum2String(), data), SetPolicy(cacheTime, slidingExpiration));
            }
            return data;
        }

        /// <summary>
        /// 设置对象数据缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        public void Set<T>(CacheKeyEnum key, T value, int? cacheTime = null, int? slidingExpiration = null)
        {
            if (value == null) { return; }
            Cache.Set(new CacheItem(key.Enum2String(), value), SetPolicy(cacheTime, slidingExpiration));
        }

        /// <summary>
        /// 设置对象数据缓存项添加删除或更改缓存后的回调方法在Controler中使用该方法需要实现接口ICacheCallBack并重写其中的方法
        /// 在类中使用该方法需要继承抽象类CacheCallBack并实现其中的方法注:该方法中删除后回调和更新后回调不可同时存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="funR"></param>
        /// <param name="funU"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        public void Set<T>(CacheKeyEnum key, T value, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null)
        {
            if (value == null) { return; }
            Cache.Set(new CacheItem(key.Enum2String(), value), SetPolicy(funR, funU, cacheTime, slidingExpiration));
        }

        /// <summary>
        /// 设置具有SqlChangeMonitor策略的CacheItemPolicy（绝对或相对过期时间为分钟）
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="sql"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        private CacheItemPolicy SetSqlPolicy(string connStr, string sql, int? cacheTime = null, int? slidingExpiration = null)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Notification = null;
                    conn.Open();
                    var dependency = new SqlDependency(command);
                    dependency.AddCommandDependency(command);
                    var monitor = new SqlChangeMonitor(dependency);
                    var policy = new CacheItemPolicy();
                    policy.ChangeMonitors.Add(monitor);
                    command.ExecuteScalar();
                    if (cacheTime.HasValue) { policy.AbsoluteExpiration = DateTime.Now.AddMinutes(cacheTime.Value); }
                    if (slidingExpiration.HasValue) { policy.SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration.Value); }
                    return policy;
                }
            }
        }

        /// <summary>
        /// 设置具有SqlChangeMonitor策略的CacheItemPolicy
        /// 并且提供当数据库表数据更改时策略被触发后删除缓存时触发的删除和更新回调函数
        /// 在Controler中使用该方法需要实现接口ICacheCallBack并重写其中的方法
        /// 在类中使用该方法需要继承抽象类CacheCallBack并实现其中的方法注:该方法中删除后回调和更新后回调不可同时存在
        /// （绝对或相对过期时间为分钟）
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="sql"></param>
        /// <param name="funR"></param>
        /// <param name="funU"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        private CacheItemPolicy SetSqlPolicy(string connStr, string sql, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null)
        {
            var sqlPolicy = SetSqlPolicy(connStr, sql, cacheTime, slidingExpiration);
            if (funR != null)
            {
                //该删除时回调方法在手动调用Remove方法时或者绝对相对时间到期时都将触发.
                sqlPolicy.RemovedCallback = new CacheEntryRemovedCallback(funR);
            }
            if (funU != null)
            {
                //该更新时回调方法只在设置的绝对或相对时间到期时将自动触发。手动调用Remove方法不会触发.
                sqlPolicy.UpdateCallback = new CacheEntryUpdateCallback(funU);
            }
            return sqlPolicy;
        }

        /// <summary>
        /// 可以提供数据库监测数据变动的缓存策略(需要设置的缓存数据为方法类型参数)（绝对或相对过期时间为分钟）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="sql"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        public void Set<T>(CacheKeyEnum key, Func<T> func, string sql, int? cacheTime = null, int? slidingExpiration = null)
        {
            var data = func();
            if (data != null)
            {
                var policy = SetSqlPolicy(connectionString, sql, cacheTime, slidingExpiration);
                Cache.Set(new CacheItem(key.Enum2String(), data), policy);
            }
        }

        /// <summary>
        /// 可以提供数据库监测数据变动的缓存策略(需要设置的缓存数据为List类型参数)（绝对或相对过期时间为分钟）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="sql"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        public void Set<T>(CacheKeyEnum key, List<T> value, string sql, int? cacheTime = null, int? slidingExpiration = null)
        {
            if (value != null)
            {
                var policy = SetSqlPolicy(connectionString, sql, cacheTime, slidingExpiration);
                Cache.Set(new CacheItem(key.Enum2String(), value), policy);
            }
        }

        /// <summary>
        /// 可以提供数据库监测数据变动的缓存策略(需要设置的缓存数据为对象类型参数)（绝对或相对过期时间为分钟）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="sql"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        public void Set<T>(CacheKeyEnum key, T t, string sql, int? cacheTime = null, int? slidingExpiration = null)
        {
            if (t != null)
            {
                var policy = SetSqlPolicy(connectionString, sql, cacheTime, slidingExpiration);
                Cache.Set(new CacheItem(key.Enum2String(), t), policy);
            }
        }

        /// <summary>
        /// 设置方法类型参数的缓存;该方法具有SqlChangeMonitor策略的CacheItemPolicy
        /// 并且提供当数据库表数据更改时策略被触发后删除缓存时触发的删除和更新回调函数
        /// 在Controler中使用该方法需要实现接口ICacheCallBack并重写其中的方法
        /// 在类中使用该方法需要继承抽象类CacheCallBack并实现其中的方法注:该方法中删除后回调和更新后回调不可同时存在
        /// （绝对或相对过期时间为分钟）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="sql"></param>
        /// <param name="funR"></param>
        /// <param name="funU"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        public void Set<T>(CacheKeyEnum key, Func<T> func, string sql, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null)
        {
            var data = func();
            if (data != null)
            {
                var policy = SetSqlPolicy(connectionString, sql, funR, funU, cacheTime, slidingExpiration);
                Cache.Set(new CacheItem(key.Enum2String(), data), policy);
            }
        }

        /// <summary>
        /// 设置List类型参数的缓存;该方法具有SqlChangeMonitor策略的CacheItemPolicy
        /// 并且提供当数据库表数据更改时策略被触发后删除缓存时触发的删除和更新回调函数
        /// 在Controler中使用该方法需要实现接口ICacheCallBack并重写其中的方法
        /// 在类中使用该方法需要继承抽象类CacheCallBack并实现其中的方法注:该方法中删除后回调和更新后回调不可同时存在
        /// （绝对或相对过期时间为分钟）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="sql"></param>
        /// <param name="funR"></param>
        /// <param name="funU"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        public void Set<T>(CacheKeyEnum key, List<T> value, string sql, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null)
        {
            if (value != null)
            {
                var policy = SetSqlPolicy(connectionString, sql, funR, funU, cacheTime, slidingExpiration);
                Cache.Set(new CacheItem(key.Enum2String(), value), policy);
            }
        }

        /// <summary>
        /// 设置对象类型参数的缓存;该方法具有SqlChangeMonitor策略的CacheItemPolicy
        /// 并且提供当数据库表数据更改时策略被触发后删除缓存时触发的删除和更新回调函数
        /// 在Controler中使用该方法需要实现接口ICacheCallBack并重写其中的方法
        /// 在类中使用该方法需要继承抽象类CacheCallBack并实现其中的方法注:该方法中删除后回调和更新后回调不可同时存在
        /// （绝对或相对过期时间为分钟）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="sql"></param>
        /// <param name="funR"></param>
        /// <param name="funU"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        public void Set<T>(CacheKeyEnum key, T t, string sql, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null)
        {
            if (t != null)
            {
                var policy = SetSqlPolicy(connectionString, sql, funR, funU, cacheTime, slidingExpiration);
                Cache.Set(new CacheItem(key.Enum2String(), t), policy);
            }
        }

        /// <summary>
        /// 判断缓存是否为空;如果为空返回True;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasCache(CacheKeyEnum key)
        {
            return Cache[key.Enum2String()] == null ? true : false;
        }
    }
}
