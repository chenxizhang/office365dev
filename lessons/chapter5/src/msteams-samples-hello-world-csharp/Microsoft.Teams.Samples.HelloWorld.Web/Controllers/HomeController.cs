using System.Text;
using System.Web.Mvc;

namespace Microsoft.Teams.Samples.HelloWorld.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("hello")]//当前作为静态选项卡用的
        public ActionResult Hello()
        {
            return View("Index");
        }

        [Route("first")]
        public ActionResult First()
        {
            return View();
        }

        [Route("second")]
        public ActionResult Second()
        {
            return View();
        }

        [Route("configure")]//配置页面
        public ActionResult Configure()
        {
            return View();
        }

        [Route("connectorconfiguration")]
        public ActionResult Connectorconfiguration()
        {
            return View();
        }
    }
}
