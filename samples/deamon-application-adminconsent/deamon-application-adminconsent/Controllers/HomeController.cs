using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deamon_application_adminconsent.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            /*
             * https://login.windows.net/59723f6b-2d14-49fe-827a-8d04f9fe7a68/oauth2/token
             * a38a723a-23c9-40e8-b4cb-23f3f337b030
             * ju9iG9RuRxdkeFkXVQ2S1EgBW2k5UUA/l3fQfpWft1E=
             */
            //https://login.microsoftonline.com/common/adminconsent?client_id=a38a723a-23c9-40e8-b4cb-23f3f337b030&state=12345&redirect_uri=http://graphadminconsent.azurewebsites.net/
            return View();
        }
    }
}