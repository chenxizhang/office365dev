/*
 * 说明：这个范例是介绍如何在Console Application中调用Microsoft Graph，获取用户基本信息，现有邮件列表，以及发送邮件
 * 作者：陈希章 Ares Chen
 * 时间：2017年4月4日
 * 参考：https://github.com/chenxizhang/office365dev 
 * 必备条件：Install-Package Microsoft.Identity.Client -IncludePrerelease
 * 必备条件：Install-Package Microsoft.Graph
 */

namespace MicrosoftGraphSample_ConsoleApplication
{
    using Microsoft.Graph;
    using Microsoft.Identity.Client;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {

            GraphSampleCode().Wait();
            Console.ReadKey();
        }

        static async Task GraphSampleCode()
        {
            var serviceClient = new GraphServiceClient(new GraphAuthenticator());

            //获取用户基本信息
            var user = await serviceClient.Me.Request().GetAsync();
            Console.WriteLine(user.DisplayName);
            Console.WriteLine(user.Mail);



            //获取用户的邮件列表
            var messages = await serviceClient.Me.MailFolders.Inbox.Messages.Request().GetAsync();
            foreach (var item in messages)
            {
                Console.WriteLine(item.Subject);
            }

            //发送邮件

            await serviceClient.Me.SendMail(new Message()
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

        }
    }


    class GraphAuthenticator : IAuthenticationProvider
    {
        static string token;
        static DateTimeOffset Expiration;



        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            string clientID = "1e545fe0-3d58-4aa8-bcfe-972930be72dc";
            string[] scopes = { "user.read", "mail.read", "mail.send" };
            var app = new PublicClientApplication(clientID);

            AuthenticationResult result = null;
            try
            {
                result = await app.AcquireTokenSilentAsync(scopes);
                token = result.Token;
            }
            catch (Exception)
            {
                if(string.IsNullOrEmpty(token) || Expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
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
