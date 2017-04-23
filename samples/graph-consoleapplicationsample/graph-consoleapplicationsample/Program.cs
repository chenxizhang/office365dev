using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;
using System.Net.Http;
using Microsoft.Identity.Client;

/// <summary>
/// 这个是国际版Microsoft Graph的客户端应用程序范例
/// 作者：陈希章
/// 时间：2017年3月23日
/// </summary>
namespace graph_consoleapplicationsample
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new GraphServiceClient(new GraphAuthenticator());//创建客户端代理
            var user = client.Me.Request().GetAsync().Result;//获取当前用户信息
            Console.WriteLine(user.DisplayName);

            var messages = client.Me.Messages.Request().GetAsync().Result;//获取用户的前十封邮件
            foreach (var item in messages)
            {
                Console.WriteLine(item.Subject);
            }

            client.Me.SendMail(new Message() //发送邮件
            {
                Subject = "调用Microsoft Graph发出的邮件",
                Body = new ItemBody()
                {
                    ContentType = BodyType.Text,
                    Content = "这是一封调用了Microsoft Graph服务发出的邮件，范例参考 https://github.com/chenxizhang/office365dev"
                },
                ToRecipients = new[]
                {
                    new Recipient()
                    {
                        EmailAddress = new EmailAddress(){ Address ="ares@office365devlabs.onmicrosoft.com"}
                    }
                }
            }, true).Request().PostAsync();


            Console.Read();

        }


        class GraphAuthenticator : IAuthenticationProvider
        {
            static string token;
            static DateTimeOffset Expiration;



            public async Task AuthenticateRequestAsync(HttpRequestMessage request)
            {
                string clientID = "45aa2ecc-5e57-4c91-86c1-b93064800c39";//这个ID是我创建的一个临时App的ID，请替换为自己的
                string[] scopes = { "user.read", "mail.read", "mail.send"};
                var app = new PublicClientApplication(clientID);

                AuthenticationResult result = null;
                try
                {
                    result = await app.AcquireTokenSilentAsync(scopes);
                    token = result.Token;
                }
                catch (Exception)
                {
                    if (string.IsNullOrEmpty(token) || Expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
                    {
                        result = await app.AcquireTokenAsync(scopes);
                        Expiration = result.ExpiresOn;
                        token = result.Token;
                    }
                }


                request.Headers.Add("Authorization", $"Bearer {token}");

            }
        }
    }
}
