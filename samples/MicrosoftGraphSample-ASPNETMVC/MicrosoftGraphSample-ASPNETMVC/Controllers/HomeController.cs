using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MicrosoftGraphSample_ASPNETMVC.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        // GET: Home
        public async Task<ActionResult> Index()
        {

            var serviceClient = SDKHelper.GetAuthenticatedClient();
            var user = await serviceClient.Me.Request().GetAsync();

            ViewBag.user = user.UserPrincipalName;
            

            return View();
        }
    }

}