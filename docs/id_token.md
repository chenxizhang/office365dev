# 解码 id_token
> 作者：陈希章 发表于 2018年3月22日

## 简介

id_token是一个特殊的token，在Microsoft Graph的认证和授权过程中颁发，它包含了已认证用户的一些信息。本文将介绍如何通过实例理解id_token，并且演示了如何解码。


## 准备环境

本文假设你已经知道如何在Azure AD中创建应用程序注册，并且在本地创建一个最简单的网站应用程序，下面这个是用asp.net core 创建的一个例子

```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using System.Text;
using System.IO;

namespace webconsole
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                
                var sb = new StringBuilder();

                foreach (var item in context.Request.Query)
                {
                    sb.AppendLine($"{item.Key}={item.Value}");
                }

                var reader = new StreamReader(context.Request.Body);
                sb.AppendLine(reader.ReadToEnd());
                

                await context.Response.WriteAsync(sb.ToString());
            });
        }
    }
}

```

通过`dotnet run`命令可以将这个应用程序运行起来。


## 如何获取id_token

id_token是一个特殊的token，在Microsoft Graph的认证和授权过程中颁发，它包含了已认证用户的一些信息。认证的协议，我们可以统一使用OpenId Connect（实际上，这是基于OAuth 的一个简单版本），授权的协议，则是采用OAuth。

如果只是需要进行身份认证，使用OpenId Connect，但需要注意的是，这种方式主要适合在Web应用中，有用户交互的情况下，你可以通过在浏览器中输入下面的地址请求用户身份认证

> https://login.microsoftonline.com/common/oauth2/authorize?client_id=`611993e2-bf37-4895-841d-9737076cdb45`&response_type=`id_token`&redirect_uri=`http://localhost:5000`&response_mode=`form_post`&scope=`openid`*&state=`12345`&nonce=`7362CAEA-9CA5-4B43-9BA3-34D7C303EBA7`

完成身份认证后，正常情况下在浏览器中可以看到如下的结果

![](images/2018-03-22-13-07-14.png)

页面上已经可以看到id_token的信息。这是一串Base64编码的文本，如下所示

`eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IkZTaW11RnJGTm9DMHNKWEdtdjEzbk5aY2VEYyIsImtpZCI6IkZTaW11RnJGTm9DMHNKWEdtdjEzbk5aY2VEYyJ9.eyJhdWQiOiI2MTE5OTNlMi1iZjM3LTQ4OTUtODQxZC05NzM3MDc2Y2RiNDUiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC81OTcyM2Y2Yi0yZDE0LTQ5ZmUtODI3YS04ZDA0ZjlmZTdhNjgvIiwiaWF0IjoxNTIxNjk0ODg5LCJuYmYiOjE1MjE2OTQ4ODksImV4cCI6MTUyMTY5ODc4OSwiYWlvIjoiWTJOZ1lMQzlKLzYzek9JNFQzekFGYyt6ejkvRnZVMElpWjNYb1pEaXREbjQ4ZmJyRElJQSIsImFtciI6WyJwd2QiXSwiZmFtaWx5X25hbWUiOiLpmYgiLCJnaXZlbl9uYW1lIjoi5biM56ugIiwiaXBhZGRyIjoiMTY3LjIyMC4yNTUuNTIiLCJuYW1lIjoi6ZmIIOW4jOeroCIsIm5vbmNlIjoiNzM2MkNBRUEtOUNBNS00QjQzLTlCQTMtMzREN0MzMDNFQkE3Iiwib2lkIjoiMmM4ZGQxMTQtZDVjYS00Nzc0LWJmMmMtNGI1NWVmMjdkNTYwIiwic3ViIjoiMmh0QlREcEZYeHh6OTBZUHRjNzRWUkktQUoydFB6bEVwU0lSY2U3RVRUTSIsInRpZCI6IjU5NzIzZjZiLTJkMTQtNDlmZS04MjdhLThkMDRmOWZlN2E2OCIsInVuaXF1ZV9uYW1lIjoiYXJlc0BvZmZpY2UzNjVkZXZsYWJzLm9ubWljcm9zb2Z0LmNvbSIsInVwbiI6ImFyZXNAb2ZmaWNlMzY1ZGV2bGFicy5vbm1pY3Jvc29mdC5jb20iLCJ1dGkiOiJRYXJBaDNKdV9rMlp2Vko3X0o4QkFBIiwidmVyIjoiMS4wIn0.AH1PI9pUMuI9J0DNOp6LVHW3yibf-b8hD3v6dSs2Pn-eGU2fi3HOY4ZU_fGSltTiVfDL-MRRispinNuhUTh3Aa9Gw936lbVs7N6zpN_SsCxIzdzq3quYxRtHoB84eXqzs7FDy53TDXtmtr89hI9wKtV2QI2pw7rBTlhuuQOxdl0638RB-eGMCtDWVj0SvK63FafazZBWdW8YSeJjf5x2XgZoNWwArGn-U5GcyTjMSywyOXJ6Ff5HssqjzuLQCtqXTL1Ouscx-M1DUyfYN-mlwHwRd3UQgUCkPgbaDebsXaz0lGXCOC61cwfkRWGjbtVLqn6DQNDlXwvggB3MTiT1TQ`


## 解码 id_token

你可能会说，这个id_token怎么完全看不懂呢？其实这是一个JSON的字符串，但是用Base64编码过的，而且分为三个部分（头部，声明，签名），用句点（.)分开的。

有一个最快的方法可以解码 id_token，就是通过jwt.ms 这个网站来查看用户信息

![](images/2018-03-22-13-10-55.png)

那么，如果要在我们的应用程序中进行解码，应该怎么做呢？其实正常情况下，使用下面的方式就可以实现

```
var id_token= "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IkZTaW11RnJGTm9DMHNKWEdtdjEzbk5aY2VEYyIsImtpZCI6IkZTaW11RnJGTm9DMHNKWEdtdjEzbk5aY2VEYyJ9.eyJhdWQiOiI2MTE5OTNlMi1iZjM3LTQ4OTUtODQxZC05NzM3MDc2Y2RiNDUiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC81OTcyM2Y2Yi0yZDE0LTQ5ZmUtODI3YS04ZDA0ZjlmZTdhNjgvIiwiaWF0IjoxNTIxNjk0ODg5LCJuYmYiOjE1MjE2OTQ4ODksImV4cCI6MTUyMTY5ODc4OSwiYWlvIjoiWTJOZ1lMQzlKLzYzek9JNFQzekFGYyt6ejkvRnZVMElpWjNYb1pEaXREbjQ4ZmJyRElJQSIsImFtciI6WyJwd2QiXSwiZmFtaWx5X25hbWUiOiLpmYgiLCJnaXZlbl9uYW1lIjoi5biM56ugIiwiaXBhZGRyIjoiMTY3LjIyMC4yNTUuNTIiLCJuYW1lIjoi6ZmIIOW4jOeroCIsIm5vbmNlIjoiNzM2MkNBRUEtOUNBNS00QjQzLTlCQTMtMzREN0MzMDNFQkE3Iiwib2lkIjoiMmM4ZGQxMTQtZDVjYS00Nzc0LWJmMmMtNGI1NWVmMjdkNTYwIiwic3ViIjoiMmh0QlREcEZYeHh6OTBZUHRjNzRWUkktQUoydFB6bEVwU0lSY2U3RVRUTSIsInRpZCI6IjU5NzIzZjZiLTJkMTQtNDlmZS04MjdhLThkMDRmOWZlN2E2OCIsInVuaXF1ZV9uYW1lIjoiYXJlc0BvZmZpY2UzNjVkZXZsYWJzLm9ubWljcm9zb2Z0LmNvbSIsInVwbiI6ImFyZXNAb2ZmaWNlMzY1ZGV2bGFicy5vbm1pY3Jvc29mdC5jb20iLCJ1dGkiOiJRYXJBaDNKdV9rMlp2Vko3X0o4QkFBIiwidmVyIjoiMS4wIn0.AH1PI9pUMuI9J0DNOp6LVHW3yibf-b8hD3v6dSs2Pn-eGU2fi3HOY4ZU_fGSltTiVfDL-MRRispinNuhUTh3Aa9Gw936lbVs7N6zpN_SsCxIzdzq3quYxRtHoB84eXqzs7FDy53TDXtmtr89hI9wKtV2QI2pw7rBTlhuuQOxdl0638RB-eGMCtDWVj0SvK63FafazZBWdW8YSeJjf5x2XgZoNWwArGn-U5GcyTjMSywyOXJ6Ff5HssqjzuLQCtqXTL1Ouscx-M1DUyfYN-mlwHwRd3UQgUCkPgbaDebsXaz0lGXCOC61cwfkRWGjbtVLqn6DQNDlXwvggB3MTiT1TQ";


var token_parts = id_token.Split('.');

var header = Encoding.UTF8.GetString(Convert.FromBase64String(token_parts[0]));
var claims = Encoding.UTF8.GetString(Convert.FromBase64String(token_parts[1]));

Console.WriteLine(header);
Console.WriteLine(claims);
```

但是上述代码，有时候会报错，我发现可能是用户信息中包含了中文的原因。但是可以通过一些第三方的库来实现完美的解码，例如`Atom.Module.Base64Url`这个package。下面是一个完整的例子：

```
using System;
using System.Text;
using Atom.Toolbox;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            var id_token= "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IkZTaW11RnJGTm9DMHNKWEdtdjEzbk5aY2VEYyIsImtpZCI6IkZTaW11RnJGTm9DMHNKWEdtdjEzbk5aY2VEYyJ9.eyJhdWQiOiI2MTE5OTNlMi1iZjM3LTQ4OTUtODQxZC05NzM3MDc2Y2RiNDUiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC81OTcyM2Y2Yi0yZDE0LTQ5ZmUtODI3YS04ZDA0ZjlmZTdhNjgvIiwiaWF0IjoxNTIxNjk0ODg5LCJuYmYiOjE1MjE2OTQ4ODksImV4cCI6MTUyMTY5ODc4OSwiYWlvIjoiWTJOZ1lMQzlKLzYzek9JNFQzekFGYyt6ejkvRnZVMElpWjNYb1pEaXREbjQ4ZmJyRElJQSIsImFtciI6WyJwd2QiXSwiZmFtaWx5X25hbWUiOiLpmYgiLCJnaXZlbl9uYW1lIjoi5biM56ugIiwiaXBhZGRyIjoiMTY3LjIyMC4yNTUuNTIiLCJuYW1lIjoi6ZmIIOW4jOeroCIsIm5vbmNlIjoiNzM2MkNBRUEtOUNBNS00QjQzLTlCQTMtMzREN0MzMDNFQkE3Iiwib2lkIjoiMmM4ZGQxMTQtZDVjYS00Nzc0LWJmMmMtNGI1NWVmMjdkNTYwIiwic3ViIjoiMmh0QlREcEZYeHh6OTBZUHRjNzRWUkktQUoydFB6bEVwU0lSY2U3RVRUTSIsInRpZCI6IjU5NzIzZjZiLTJkMTQtNDlmZS04MjdhLThkMDRmOWZlN2E2OCIsInVuaXF1ZV9uYW1lIjoiYXJlc0BvZmZpY2UzNjVkZXZsYWJzLm9ubWljcm9zb2Z0LmNvbSIsInVwbiI6ImFyZXNAb2ZmaWNlMzY1ZGV2bGFicy5vbm1pY3Jvc29mdC5jb20iLCJ1dGkiOiJRYXJBaDNKdV9rMlp2Vko3X0o4QkFBIiwidmVyIjoiMS4wIn0.AH1PI9pUMuI9J0DNOp6LVHW3yibf-b8hD3v6dSs2Pn-eGU2fi3HOY4ZU_fGSltTiVfDL-MRRispinNuhUTh3Aa9Gw936lbVs7N6zpN_SsCxIzdzq3quYxRtHoB84eXqzs7FDy53TDXtmtr89hI9wKtV2QI2pw7rBTlhuuQOxdl0638RB-eGMCtDWVj0SvK63FafazZBWdW8YSeJjf5x2XgZoNWwArGn-U5GcyTjMSywyOXJ6Ff5HssqjzuLQCtqXTL1Ouscx-M1DUyfYN-mlwHwRd3UQgUCkPgbaDebsXaz0lGXCOC61cwfkRWGjbtVLqn6DQNDlXwvggB3MTiT1TQ";


            var token_parts = id_token.Split('.');

            var header = Encoding.UTF8.GetString(Base64Url.Decode(token_parts[0]));
            var claims = Encoding.UTF8.GetString(Base64Url.Decode(token_parts[1]));

            Console.WriteLine(header);
            Console.WriteLine(claims);



            Console.Read();

            
        }
    }
}


```

![](images/2018-03-22-13-51-10.png)