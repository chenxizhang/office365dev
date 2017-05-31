using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace daemonapplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //准备环境
            var clientId = "dff48006-b010-4859-b5d5-68acdb821322";
            var client_secret = "uxO3frQOekCfdOfX2Oom4Vc";
            var tenantId = "59723f6b-2d14-49fe-827a-8d04f9fe7a68";

            
            var client = new HttpClient();

            //获得令牌
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token");
            var body = new StringContent($"grant_type=client_credentials&client_id={clientId}&scope=https%3A%2F%2Fgraph.microsoft.com%2F.default&client_secret={client_secret}");
            body.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
            request.Content = body;
            var access_token = JObject.Parse(client.SendAsync(request).Result.Content.ReadAsStringAsync().Result)["access_token"].ToString();

            //访问资源
            request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access_token);

            var users = JObject.Parse(client.SendAsync(request).Result.Content.ReadAsStringAsync().Result)["value"];

            foreach (var item in users)
            {
                Console.WriteLine($"displayName:{item["displayName"]},email:{item["email"]}");
            }

            Console.Read();


        }
    }
}
