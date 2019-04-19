using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Comm.EnumExtent.CacheEnum
{
    /// <summary>
    /// 针对缓存key枚举CacheKeyEnum的扩展方法
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static class CacheKeyEnumExtent
    {
        /// <summary>
        /// CacheKeyEnum转换string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Enum2String(this CacheKeyEnum key)
        {
            return Enum.GetName(typeof(CacheKeyEnum), key);
        }

        /// <summary>
        /// 数字转换枚举
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        //public static CacheKeyEnum IntConvert2Enum(this int num)
        //{
        //    if (Enum.IsDefined(typeof(CacheKeyEnum), num))
        //    {
        //        return (CacheKeyEnum)Enum.ToObject(typeof(CacheKeyEnum), num);
        //    }
        //    return CacheKeyEnum.AlarmKey;
        //}

        /// <summary>
        /// string转换枚举
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //public static CacheKeyEnum StringConvert2Enum(this string str)
        //{
        //    CacheKeyEnum cacheKey = CacheKeyEnum.AlarmKey;
        //    try
        //    {
        //        cacheKey = (CacheKeyEnum)Enum.Parse(typeof(CacheKeyEnum), str);
        //    }
        //    catch (Exception ex)
        //    {
        //        return cacheKey;
        //    }

        //    return cacheKey;
        //}

        /// <summary>
        /// 枚举转换数字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int EnumConvert2Int(this CacheKeyEnum key)
        {
            return (int)key;
        }
    }
}
