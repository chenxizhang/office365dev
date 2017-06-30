using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Graph;
using System.Net.Http.Headers;
using Office365GraphCoreMVCHelper;
using Microsoft.Extensions.Options;

namespace aspntecoremvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<AppSetting> Options;
        public HomeController(IOptions<AppSetting> options)
        {
            this.Options = options;
        }
    
        public IActionResult Index()
        {

            return View();
        }

        [Authorize]
        public async Task<IActionResult> About()
        {
            var client = await this.GetAuthenticatedClient(this.Options);            
            return View(await client.Me.Request().GetAsync());
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
