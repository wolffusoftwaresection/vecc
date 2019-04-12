using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Comm.NlogExtent
{
    public class LogUtil
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void LogDebug(string message)
        {
            logger.Log(LogLevel.Debug, message);
        }

        public static void LogError(string message)
        {
            logger.Log(LogLevel.Error, message);
        }
        public void LogErrorException(Exception ex)
        {
            LogException(LogLevel.Error, ex);
        }

        public void LogFatalException(Exception ex)
        {
            LogException(LogLevel.Fatal, ex);
        }

        public void LogFatal(string message)
        {
            logger.Log(LogLevel.Fatal, message);
        }

        public static void LogInfo(LogEventInfo theEvent)
        {
            logger.Log(theEvent);
        }

        public static void LogException(LogEventInfo theEvent)
        {
            logger.Log(theEvent);
        }
        public static void LogInfo(string message)
        {
            logger.Log(LogLevel.Info, message);
        }

        public void LogOff(string message)
        {
            logger.Log(LogLevel.Off, message);
        }

        public void LogTrace(string message)
        {
            logger.Log(LogLevel.Trace, message);
        }

        public void LogWarn(string message)
        {
            logger.Log(LogLevel.Warn, message);
        }

        private static void LogException(LogLevel level, Exception ex)
        {
            logger.Log(level, GetExceptionMessage(ex));
        }

        private static string GetExceptionMessage(Exception ex)
        {
            string message = ex.Message;
            string stackTrace = ex.StackTrace;
            if (string.IsNullOrEmpty(stackTrace) && ex.InnerException != null)
            {
                stackTrace = ex.InnerException.StackTrace;
            }
            return message + "::" + stackTrace;
        }
    }
}
