using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;
using System.Collections.Generic;

namespace clientapp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region parameters
            string authority, resource, deviceloginaddr;
            authority = "https://login.chinacloudapi.cn/common";
            resource = "https://modtsp.partner.onmschina.cn/dotnet-obo-webapi";
            deviceloginaddr = "https://aka.ms/gallatindevicelogin";
            #endregion

            var ctx = new AuthenticationContext(authority: authority);
            var deviceCode = ctx.AcquireDeviceCodeAsync(resource, "5ba82619-4604-4b1b-9a73-86cd1796cbaf").Result;

            Console.WriteLine($"请打开浏览器，访问 {deviceloginaddr} ,并以 {deviceCode.UserCode} 登陆");
            var result = ctx.AcquireTokenByDeviceCodeAsync(deviceCode).Result;



            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            // Call the To Do list service.
            HttpResponseMessage response = httpClient.GetAsync("http://localhost:5000/api/values").Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}
