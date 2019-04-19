using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Comm.EnumExtent.CacheEnum;

namespace Wolf.Vecc.Core.Cache
{
    public interface ICacheManager
    {
        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(CacheKeyEnum key);

        /// <summary>
        /// 获取List缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> GetList<T>(CacheKeyEnum key);

        /// <summary>
        /// 判断缓存是否为空
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool HasCache(CacheKeyEnum key);

        /// <summary>
        /// 常用方法如果缓存过期为null可传入重新获取数据方法并设置缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        T Get<T>(CacheKeyEnum key, Func<T> func, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null);

        /// <summary>
        /// 常用方法如果缓存过期为null可传入重新获取数据方法并设置缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        List<T> GetList<T>(CacheKeyEnum key, Func<List<T>> func, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null);

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        void Set<T>(CacheKeyEnum key, List<T> value, int? cacheTime = null, int? slidingExpiration = null);

        /// <summary>
        /// 设置缓存项添加删除或更改缓存后的回调方法在Controler中使用该方法需要实现接口ICacheCallBack并重写其中的方法
        /// 在类中使用该方法需要继承抽象类CacheCallBack并实现其中的方法注:该方法中删除后回调和更新后回调不可同时存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="funR"></param>
        /// <param name="funU"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        void Set<T>(CacheKeyEnum key, List<T> value, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null);

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        void Set<T>(CacheKeyEnum key, Func<T> func, int? cacheTime = null, int? slidingExpiration = null);

        /// <summary>
        /// 设置缓存项添加删除或更改缓存后的回调方法在Controler中使用该方法需要实现接口ICacheCallBack并重写其中的方法
        /// 在类中使用该方法需要继承抽象类CacheCallBack并实现其中的方法注:该方法中删除后回调和更新后回调不可同时存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="funT"></param>
        /// <param name="funR"></param>
        /// <param name="funU"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        void Set<T>(CacheKeyEnum key, Func<T> funT, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null);

        /// <summary>
        /// 设置对象数据缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        void Set<T>(CacheKeyEnum key, T value, int? cacheTime = null, int? slidingExpiration = null);

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
        void Set<T>(CacheKeyEnum key, T value, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null);

        /// <summary>
        /// 缓存key是否已存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsSet(CacheKeyEnum key);

        /// <summary>
        /// 删除指定缓存项
        /// </summary>
        /// <param name="key"></param>
        void Remove(CacheKeyEnum key);

        /// <summary>
        /// 清空所有缓存项
        /// </summary>
        void Clear();

        /// <summary>
        /// 最新方法如果缓存过时为空将使用传入的方法获取需要设置的最新数据进行缓存
        /// 该方法将返回对象数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        T Get<T>(CacheKeyEnum key, Func<T> func);

        /// <summary>
        /// 最新方法如果缓存过时为空将使用传入的方法获取需要设置的最新数据进行缓存
        /// 该方法将返回List数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        List<T> Get<T>(CacheKeyEnum key, Func<List<T>> func);

        /// <summary>
        /// 可以提供数据库监测数据变动的缓存策略(需要设置的缓存数据为方法类型参数)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="sql"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        void Set<T>(CacheKeyEnum key, Func<T> func, string sql, int? cacheTime = null, int? slidingExpiration = null);

        /// <summary>
        /// 可以提供数据库监测数据变动的缓存策略(需要设置的缓存数据为List类型参数)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="sql"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        void Set<T>(CacheKeyEnum key, List<T> value, string sql, int? cacheTime = null, int? slidingExpiration = null);

        /// <summary>
        /// 可以提供数据库监测数据变动的缓存策略(需要设置的缓存数据为对象类型参数)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="sql"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        void Set<T>(CacheKeyEnum key, T t, string sql, int? cacheTime = null, int? slidingExpiration = null);

        /// <summary>
        /// 可以提供数据库监测数据变动的缓存策略(需要设置的缓存数据为方法类型参数)
        /// 并当sql缓存策略触发时(删除缓存时)提供删除或更新的回调函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="sql"></param>
        /// <param name="funR"></param>
        /// <param name="funU"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        void Set<T>(CacheKeyEnum key, Func<T> func, string sql, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null);

        /// <summary>
        /// 可以提供数据库监测数据变动的缓存策略(需要设置的缓存数据为List类型参数)
        /// 并当sql缓存策略触发时(删除缓存时)提供删除或更新的回调函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="sql"></param>
        /// <param name="funR"></param>
        /// <param name="funU"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        void Set<T>(CacheKeyEnum key, List<T> value, string sql, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null);

        /// <summary>
        /// 可以提供数据库监测数据变动的缓存策略(需要设置的缓存数据为对象类型参数)
        /// 并当sql缓存策略触发时(删除缓存时)提供删除或更新的回调函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="sql"></param>
        /// <param name="funR"></param>
        /// <param name="funU"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        void Set<T>(CacheKeyEnum key, T t, string sql, Action<CacheEntryRemovedArguments> funR, Action<CacheEntryUpdateArguments> funU, int? cacheTime = null, int? slidingExpiration = null);
    }
}
