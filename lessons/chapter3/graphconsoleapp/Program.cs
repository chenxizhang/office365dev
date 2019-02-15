/*
作者：陈希章 Ares Chen
时间：2018年4月21日
说明：
    这是一个可以快速通过Microsoft Graph访问到Office 365资源的控制台应用程序模板。
    它使用基于设备的身份验证方式，并且同时支持国际版和国内版。

关于此模板的使用以及问题反馈，请访问 https://github.com/chenxizhang/dotnetcore-office365dev-templates/tree/master/dotnetcore-graph-console
Office 365开发入门指南，请参考 https://github.com/chenxizhang/office365dev 
更多模板请参考 https://github.com/chenxizhang/dotnetcore-office365dev-templates 
*/
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace graphconsoleapp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region parameters
            string authority,resource,deviceloginaddr;
            authority ="https://login.microsoftonline.com/common/oauth2";
            resource="https://graph.microsoft.com";
            deviceloginaddr="https://aka.ms/devicelogin";     
       
            #endregion



            var token = "";

            var client = new GraphServiceClient(new DelegateAuthenticationProvider((request) =>
            {
                if (string.IsNullOrEmpty(token))
                {
                    var ctx = new AuthenticationContext(authority: authority);
                    var deviceCode = ctx.AcquireDeviceCodeAsync(resource, "cfc3a225-ac57-45c9-aacd-969551f4825f").Result;
                    Console.WriteLine($"请打开浏览器，访问{deviceloginaddr},并以 {deviceCode.UserCode} 登陆");
                    token = ctx.AcquireTokenByDeviceCodeAsync(deviceCode).Result.AccessToken;
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return Task.FromResult(0);
            }))
            {
                BaseUrl = $"{resource}/v1.0"
            };

            var user = client.Me.Request().GetAsync().Result;
            Console.WriteLine($"当前登录用户为:{user.DisplayName}\r\n");

            var messages = client.Me.Messages.Request().GetAsync().Result;
            Console.WriteLine($"当前用户的收件箱最近十封邮件如下：");
            foreach (var item in messages)
            {
                Console.WriteLine($"\t{item.Subject}");
            }

            var files = client.Me.Drive.Root.Children.Request().GetAsync().Result;
            Console.WriteLine($"\r\n当前用户的个人网盘的文件信息如下：");
            foreach (var item in files)
            {
                Console.WriteLine($"\t{item.Name}");
            }

        }
    }
}
