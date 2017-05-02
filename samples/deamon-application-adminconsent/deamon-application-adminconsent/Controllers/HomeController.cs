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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            /*
             * https://login.windows.net/59723f6b-2d14-49fe-827a-8d04f9fe7a68/oauth2/token
             * a38a723a-23c9-40e8-b4cb-23f3f337b030
             * ju9iG9RuRxdkeFkXVQ2S1EgBW2k5UUA/l3fQfpWft1E=
             */
            //https://login.microsoftonline.com/59723f6b-2d14-49fe-827a-8d04f9fe7a68/adminconsent?client_id=09082ce3-13c3-4d15-9799-431316d8b728&state=12345&redirect_uri=https://graphadminconsent.azurewebsites.net/



            /*
            POST https://login.microsoftonline.com/59723f6b-2d14-49fe-827a-8d04f9fe7a68/oauth2/token HTTP/1.1
            Content - Type: application / x - www - form - urlencoded

            grant_type = client_credentials
            & client_id =09082ce3-13c3-4d15-9799-431316d8b728
            &client_secret =+USDZqA76m2EFBSj2T/IaHNosf7adJSLEtVUoE5/MHM=
            &resource = https://graph.microsoft.com 
            */

            /*
            GET https://graph.microsoft.com/v1.0/users
            Authorization: Bearer <token>
            */

            return View();
        }
    }
}
 