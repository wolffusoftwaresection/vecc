using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Comm.Helpers
{
    public class Office2HtmlHelper
    { 
      /// <summary>
      /// Word转成Html
      /// </summary>
      /// <param name="path">要转换的文档的路径</param>
      /// <param name="savePath">转换成html的保存路径</param>
      /// <param name="wordFileName">转换成html的文件名字</param>
        //public static void Word2Html(string path, string savePath, string wordFileName)
        //{
        //    Microsoft.Office.Interop.Word.ApplicationClass word = new Microsoft.Office.Interop.Word.ApplicationClass();
        //    Type wordType = word.GetType();
        //    Microsoft.Office.Interop.Word.Documents docs = word.Documents;
        //    Type docsType = docs.GetType();
        //    Microsoft.Office.Interop.Word.Document doc = (Microsoft.Office.Interop.Word.Document)docsType.InvokeMember("Open", System.Reflection.BindingFlags.InvokeMethod, null, docs, new Object[] { (object)path, true, true });
        //    Type docType = doc.GetType();
        //    string strSaveFileName = savePath + wordFileName + ".html";
        //    object saveFileName = (object)strSaveFileName;
        //    docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, doc, new object[] { saveFileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatFilteredHTML });
        //    docType.InvokeMember("Close", System.Reflection.BindingFlags.InvokeMethod, null, doc, null);
        //    wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, word, null);
        //}

        #region 预览Excel
        /// <summary>
        /// 预览Excel
        /// </summary>
        //public string PreviewExcel(string physicalPath, string url)
        //{
        //    Microsoft.Office.Interop.Excel.Application application = null;
        //    Microsoft.Office.Interop.Excel.Workbook workbook = null;
        //    application = new Microsoft.Office.Interop.Excel.Application();
        //    object missing = Type.Missing;
        //    object trueObject = true;
        //    application.Visible = false;
        //    application.DisplayAlerts = false;
        //    workbook = application.Workbooks.Open(physicalPath, missing, trueObject, missing, missing, missing,
        //        missing, missing, missing, missing, missing, missing, missing, missing, missing);
        //    //Save Excel to Html
        //    object format = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
        //    string htmlName = Path.GetFileNameWithoutExtension(physicalPath) + ".html";
        //    String outputFile = Path.GetDirectoryName(physicalPath) + "\\" + htmlName;
        //    workbook.SaveAs(outputFile, format, missing, missing, missing,
        //                      missing, XlSaveAsAccessMode.xlNoChange, missing,
        //                      missing, missing, missing, missing);
        //    workbook.Close();
        //    application.Quit();
        //    return Path.GetDirectoryName(Server.UrlDecode(url)) + "\\" + htmlName;
        //}
        #endregion

        #region 预览Word
        /// <summary>
        /// 预览Word
        /// </summary>
        //public string PreviewWord(string physicalPath, string url)
        //{
        //    Microsoft.Office.Interop.Word._Application application = null;
        //    Microsoft.Office.Interop.Word._Document doc = null;
        //    application = new Microsoft.Office.Interop.Word.Application();
        //    object missing = Type.Missing;
        //    object trueObject = true;
        //    application.Visible = false;
        //    application.DisplayAlerts = WdAlertLevel.wdAlertsNone;
        //    doc = application.Documents.Open(physicalPath, missing, trueObject, missing, missing, missing,
        //        missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
        //    //Save Excel to Html
        //    object format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML;
        //    string htmlName = Path.GetFileNameWithoutExtension(physicalPath) + ".html";
        //    String outputFile = Path.GetDirectoryName(physicalPath) + "\\" + htmlName;
        //    doc.SaveAs(outputFile, format, missing, missing, missing,
        //                      missing, XlSaveAsAccessMode.xlNoChange, missing,
        //                      missing, missing, missing, missing);
        //    doc.Close();
        //    application.Quit();
        //    return Path.GetDirectoryName(Server.MapPath.UrlDecode(url)) + "\\" + htmlName;
        //}
        #endregion

        #region 预览Txt
        /// <summary>
        /// 预览Txt
        /// </summary>
        //public string PreviewTxt(string physicalPath, string url)
        //{
        //    return Server.UrlDecode(url);
        //}
        #endregion

        #region 预览Pdf
        /// <summary>
        /// 预览Pdf
        /// </summary>
        //public string PreviewPdf(string physicalPath, string url)
        //{
        //    return Server.UrlDecode(url);
        //}
        #endregion

        #region 预览图片
        /// <summary>
        /// 预览图片
        /// </summary>
        //public string PreviewImg(string physicalPath, string url)
        //{
        //    return Server.UrlDecode(url);
        //}
        #endregion

        #region 预览其他文件
        /// <summary>
        /// 预览其他文件
        /// </summary>
        //public string PreviewOther(string physicalPath, string url)
        //{
        //    return Server.UrlDecode(url);
        //}
        #endregion
    }
}