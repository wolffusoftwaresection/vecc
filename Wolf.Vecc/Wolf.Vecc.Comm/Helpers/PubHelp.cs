using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Comm.Helpers
{
    public static class PubHelp
    {
        public static string GetStatus(int taskstatus)
        {
            //任务状态，0等待处理，1正在校验，2校验完成，3正在转换文件，4转换文件完成，5正在计算，6计算完成，7生成报完成
            //-21示数据校验失败，-22表示文件转换失败，-23表示计算失败，-24表示报告生成失败，-25系统异常
            string statusstr = "";
            if (taskstatus == 0)
            {
                statusstr += "导入的测试数据正等待处理……";
            }
            else if (taskstatus == 1)
            {
                statusstr += "导入的测试数据正在校验……";
            }
            else if (taskstatus == 2)
            {
                statusstr += "导入的测试数据校验完成";
            }
            else if (taskstatus == 3)
            {
                statusstr += "导入的测试数据正在转换文件……";
            }
            else if (taskstatus == 4)
            {
                statusstr += "导入的测试数据转换完成";
            }
            else if (taskstatus == 5)
            {
                statusstr += "导入的测试数据正在计算……";
            }
            else if (taskstatus == 6)
            {
                statusstr += "导入的测试数据计算完成";
            }
            else if (taskstatus == 7)
            {
                statusstr += "生成报告完成";
            }
            else if (taskstatus == -21)
            {
                statusstr += "数据校验失败";
            }
            else if (taskstatus == -22)
            {
                statusstr += "导入的测试文件格式转换失败";
            }
            else if (taskstatus == -23)
            {
                statusstr += "导入的测试数据计算失败";
            }
            else if (taskstatus == -24)
            {
                statusstr = "报告生成失败";
            }
            else if (taskstatus == -25)
            {
                statusstr += "系统异常";
            }
            return statusstr;
        }

        public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }

        public static string PostData(string theurl, string postdata)
        {
            try
            {
                LogHelper.LogInfo("--------执行postData---------");
                LogHelper.LogInfo(theurl);
                byte[] postData = Encoding.UTF8.GetBytes(postdata);
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(theurl); ;
                request.Method = "POST";
                request.KeepAlive = false;
                request.AllowAutoRedirect = true;
                request.ContentType = "application/json";
                request.Timeout = 3000;
                request.ContentLength = postData.Length;

                System.IO.Stream outputStream = request.GetRequestStream();
                outputStream.Write(postData, 0, postData.Length);
                outputStream.Close();

                System.Net.HttpWebResponse response;
                Stream responseStream;
                StreamReader reader;
                string srcString;
                response = request.GetResponse() as System.Net.HttpWebResponse;
                responseStream = response.GetResponseStream();
                reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                srcString = reader.ReadToEnd();
                string result = srcString;   //返回值赋值
                reader.Close();
                return result;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static string GetData(string theurl)
        {
            try
            {
                Uri uri = new Uri(theurl);
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
                request.Method = "Get";
                request.ContentType = "application/x-www-form-urlencoded";
                request.AllowAutoRedirect = false;
                request.Timeout = 5000;
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                string retext = readStream.ReadToEnd().ToString();
                readStream.Close();
                return retext;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static string DeleteData(string theurl, string postdata)
        {
            try
            {
                byte[] postData = Encoding.UTF8.GetBytes(postdata);
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(theurl); ;
                request.Method = "DELETE";
                request.KeepAlive = false;
                request.AllowAutoRedirect = true;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 3000;
                request.ContentLength = postData.Length;

                System.IO.Stream outputStream = request.GetRequestStream();
                outputStream.Write(postData, 0, postData.Length);
                outputStream.Close();

                System.Net.HttpWebResponse response;
                Stream responseStream;
                StreamReader reader;
                string srcString;
                response = request.GetResponse() as System.Net.HttpWebResponse;
                responseStream = response.GetResponseStream();
                reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                srcString = reader.ReadToEnd();
                string result = srcString;   //返回值赋值
                reader.Close();
                return result;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
