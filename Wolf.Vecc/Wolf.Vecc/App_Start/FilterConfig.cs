using System.Web;
using System.Web.Mvc;
using Wolf.Vecc.Filters;

namespace Wolf.Vecc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionFilter());
            filters.Add(new UserTrackerLogAttribute());
        }
    }
}
