using System.Web;
using System.Web.Mvc;

namespace Microsoft_Graph_REST_ASPNET_Connect
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
