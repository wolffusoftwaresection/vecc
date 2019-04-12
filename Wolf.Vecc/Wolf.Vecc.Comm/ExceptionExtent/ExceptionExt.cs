using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Comm.ExceptionExtent
{
    public static class ExceptionExt
    {
        public static string ExceptionMsg(Exception exp)
        {
            //获取ex的第一级内部异常
            Exception innerEx = exp.InnerException == null ? exp : exp.InnerException;

            StringBuilder sbExMsg = new StringBuilder();
            sbExMsg.Append(innerEx.Message + "\n");

            //循环获取内部异常直到获取详细异常信息为止
            while (innerEx.InnerException != null)
            {
                innerEx = innerEx.InnerException;
                sbExMsg.Append(innerEx.Message + "\n");
            }
            return sbExMsg.ToString();
        }

        public static string ExceptionStackTrace(this Exception exp)
        {
            var _exp = exp.InnerException == null ? exp : exp.InnerException;
            return _exp.StackTrace;
        }
    }
}
