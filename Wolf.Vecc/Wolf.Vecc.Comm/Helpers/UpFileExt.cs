using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Wolf.Vecc.Comm.Helpers
{
    public static class UpFileExt
    {
        /// <summary>
        /// 传入当前项目的文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static bool CreateFolder(string folder)
        {
            bool status = true;
            string fileUrl = System.Configuration.ConfigurationSettings.AppSettings["FileUrl"].ToString();
            string projectUrl = fileUrl + folder;
            if (Directory.Exists(projectUrl) == false)
            {
                try
                {
                    Directory.CreateDirectory(projectUrl);
                    string originalUrl = projectUrl + "/" + "OriginalFile";
                    if (Directory.Exists(originalUrl) == false)
                    {
                        Directory.CreateDirectory(originalUrl);
                        string resultUrl = projectUrl + "/" + "ResultFiles";
                        if (Directory.Exists(resultUrl) == false)
                        {
                            Directory.CreateDirectory(resultUrl);
                        }
                    }
                }
                catch
                {
                    status = false;
                }
            }
            return status;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="httpPostedFile">待上传的文件</param>
        /// <param name="filesurl">需要传入的文件夹</param>
        /// <returns></returns>
        public static string UpLoadFile(HttpPostedFile httpPostedFile, string filesurl)
        {
            string FileName = "";
            string FileUrl = GlobalConfigHelper.GetFileUrl();
            if (Directory.Exists(filesurl) == false)
            {
                Directory.CreateDirectory(FileUrl + "/" + filesurl);
            }
            Int32 FileCount = httpPostedFile.ContentLength;
            if (FileCount > 0)
            {
                FileName = httpPostedFile.FileName;
                FileName = FileName.Substring(FileName.LastIndexOf(".") + 1);
                FileName = System.DateTime.Now.Ticks.ToString() + "." + FileName;
                httpPostedFile.SaveAs(FileUrl + "/" + filesurl + "/" + FileName);
            }
            string resultrul = FileName;
            return resultrul;
        }
    }
}
