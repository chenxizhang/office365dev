using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 这个是国际版Microsoft Graph的客户端应用程序范例
/// 作者：陈希章
/// 时间：2017年3月23日
/// </summary>
namespace graph_consoleapplicationsample_china
{
    class Program
    {
        static void Main(string[] args)
        {
            ///获得用户的令牌
            var token = GetAccessToken().Result;

            //获得用户的基本信息
            var me = InvokeRestReqeust("https://microsoftgraph.chinacloudapi.cn/v1.0/me", token).Result;
            Console.WriteLine(me);

            //获得用户的邮件列表（前十封）
            var messages = InvokeRestReqeust("https://microsoftgraph.chinacloudapi.cn/v1.0/me/messages", token).Result;
            Console.WriteLine(messages);


            Console.Read();

        }

        /// <summary>
        /// 定义这个方法用来进行Rest调用
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        static async Task<string> InvokeRestReqeust(string url, string token)
        {
            var client = new System.Net.WebClient();
            client.Headers.Add("Authorization", $"Bearer {token}");
            var result = await client.DownloadStringTaskAsync(url);
            return result;//请注意，这里直接返回字符串型的结果，它是Json格式的，有兴趣的可以继续在这个基础上进行处理
        }


        static async Task<string> GetAccessToken()
        {
            var appId = "9c7dd51c-072c-4aea-aaee-fc57efacb150";
            var authorizationEndpoint = "https://login.chinacloudapi.cn/common/oauth2/authorize";//国际版是https://login.microsoftonline.com/common/oauth2/authorize
            var resource = "https://microsoftgraph.chinacloudapi.cn"; //国际版是https://graph.microsoft.com 
            var redirectUri = "http://nativeapplication";//其实这个应该去掉，目前必须要填，而且要跟注册时一样


            AuthenticationResult result = null;

            var context = new AuthenticationContext(authorizationEndpoint);
            result = await context.AcquireTokenAsync(resource, appId, new Uri(redirectUri), new PlatformParameters(PromptBehavior.Always));


            return result.AccessToken;
        }
    }
}
