using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wolf.Vecc.Comm.Helpers
{
    public static class RenderViewTostring
    {
        /// <summary>
        ///将部分视图转成html 字符串方便我们扩展使用
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static string RenderPartialView(this Controller controller, string viewName)
        {
            return RenderPartialView(controller, viewName, null);
        }
        /// <summary>
        ///将部分视图转成html 字符串方便我们扩展使用
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static string RenderPartialView(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;
            //TempDataDictionary tempdata = new TempDataDictionary();
            //tempdata.Add("Result", json);
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                // return sw.Encoding.EncodingName;
                return sw.GetStringBuilder().ToString();
            }
        }
        /// <summary>
        /// 部分视图转成html 
        /// viewbag 和viewdata 访问是相通的.
        /// ViewData["Hello"] = "Hello Boy!";
        /// view ViewBag.Hello 就是访问上面这个;
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <param name="viewdataary">键值形式的对象</param>
        /// <returns></returns>
        public static string RenderPartialView(this Controller controller, string viewName, object model, params KeyValuePair<string, object>[] viewdataary)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
            controller.ViewData.Model = model;
            if (viewdataary != null && viewdataary.Any())
            {
                foreach (var item in viewdataary)
                {
                    controller.ViewData.Add(item);
                }
            }
            return ControllserRenderView(controller, viewName);
        }

        /// <summary>
        /// Controllsers the render view.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <returns></returns>
        private static string ControllserRenderView(Controller controller, string viewName)
        {
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
