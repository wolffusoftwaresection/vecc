using System;
using System.Collections.Generic;
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
        public static void Word2Html(string path, string savePath, string wordFileName)
        {
            Microsoft.Office.Interop.Word.ApplicationClass word = new Microsoft.Office.Interop.Word.ApplicationClass();
            Type wordType = word.GetType();
            Microsoft.Office.Interop.Word.Documents docs = word.Documents;
            Type docsType = docs.GetType();
            Microsoft.Office.Interop.Word.Document doc = (Microsoft.Office.Interop.Word.Document)docsType.InvokeMember("Open", System.Reflection.BindingFlags.InvokeMethod, null, docs, new Object[] { (object)path, true, true });
            Type docType = doc.GetType();
            string strSaveFileName = savePath + wordFileName + ".html";
            object saveFileName = (object)strSaveFileName;
            docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, doc, new object[] { saveFileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatFilteredHTML });
            docType.InvokeMember("Close", System.Reflection.BindingFlags.InvokeMethod, null, doc, null);
            wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, word, null);
        }
    }
}
}
