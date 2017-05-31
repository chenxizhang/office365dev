# 在无人值守程序（服务）中调用Micrsooft Graph

> 作者：陈希章 发表于 2017年5月31日

## 什么是无人值守程序（服务）

我在此前用了几篇文章分别介绍了在桌面应用程序（控制台），Web应用程序（ASP.NET MVC），以及PowerSehll脚本中如何访问Microsoft Graph，今天这一篇要继续讲一个场景：在无人值守程序中访问Microsoft Graph。那么什么是无人值守程序呢？通常我们将此类程序定义为不需要（不允许）用户进行干预，一般用来在后台自动化运行的程序。在英文文档中，我们将其称之为daemon application，广义上说，也包括了服务这种特殊的应用程序。

无人值守程序与Microsoft Graph的集成，要遵守一般的流程，但也有自己的一些特点，总结起来有如下的步骤

1. 注册应用程序
1. 配置应用程序权限
1. 获得管理员同意
1. 获得访问令牌
1. 使用令牌访问资源

> 关于这个话题，官方有一个英文的文档，请参考 <https://developer.microsoft.com/en-us/graph/docs/concepts/auth_v2_service>


## 注册应用程序

针对Azure AD的不同版本，注册应用程序的过程我此前已经有专门的文章介绍了，请参考

1. 注册Azure AD 2.0应用程序（支持国际版，支持Office 365账号以及个人账号登录，功能可能有所缺失，但这个是以后的方向），[文章链接](applicationregisteration2.0.md)
1. 注册Azure AD 1.0应用程序（支持国际版和中国版，仅支持Office 365账号登录，功能最全），[文章链接1](applicationregisteration.md)，[文章链接2](chinaoffice365applicationregisteration.md)

本文先演示注册Azure AD 2.0应用，具体的步骤就不做截图了，唯一要提醒的是

1. 应用程序类型设置为Web
1. ReplyUrl可以设置为一个通用的地址，我喜欢设置为 <https://developer.microsoft.com/en-us/graph/>

![](images/daemonapplicationregistration-1.PNG)

## 配置应用程序权限

这是无人值守应用程序注册的时候，要特别注意的。由于该程序是没有用户参与的，所以它无法使用某个特定用户的身份做什么事情，而是使用一个统一的身份，该身份我们称为Application Identity，而相应的，我们要为程序申请的权限也是所谓的“Application Permissions”，而不是“Delegated Permissions”。

本例中，我想为应用程序申请两个权限：一个是用来获取所有用户信息的，另外一个是用来代替任何用户发送邮件。

![](images/daemonapplication-permissions.PNG)


## 获得管理员同意

由于无人值守的程序其实是自动化运行的，无需用户进行参与进行授权，而它进行的操作，却又有可能要代表用户的行为。所以通常这些权限都需要得到真正的Office 365 Tenant管理员同意才能真正生效。

> 其实细心的朋友在上图中也应该可以看出来，几乎所有Application Permission都是需要管理员同意的（Admin Consent）

要获得管理员同意，你可以将下面的这个链接发送给用户的Office 365 Tenant管理员

<https://login.microsoftonline.com/common/adminconsent?client_id=dff48006-b010-4859-b5d5-68acdb821322&state=12345&redirect_uri=https://developer.microsoft.com/en-us/graph/>

管理员需要在下面这样的界面中对应用程序所申请的权限进行确认

![](images/daemonapplication-adminconsent.png)

正常情况下，完成授权后页面会被导航到下面的地址，请确认admin_consent的值为true，并记录下来tenant的值。这个表示用户的Office 365 Tenant的编号，后面我们需要用到。

<https://developer.microsoft.com/en-us/graph/?admin_consent=True&tenant=59723f6b-2d14-49fe-827a-8d04f9fe7a68&state=12345>


## 获取访问令牌

无人值守应用程序，不需要用户参与进行授权，所以它获取令牌的方式也略有不同。你可以在应用程序里面使用下面的方式发起一个POST请求来获得访问令牌（Access Token）。

```
POST https://login.windows.net/59723f6b-2d14-49fe-827a-8d04f9fe7a68/oauth2/token
Content-Type: application/x-www-form-urlencoded
Host: login.windows.net

client_id=338c8e70-d0da-444e-b877-9f427a16eb17&scope=https%3A%2F%2Fgraph.microsoft.com%2F.default&client_secret=8V59e4aBfNr6x4lN8EAMTisk3J7WRH+glZbvgMwdDQY=&grant_type=client_credentials
```

![](images/daemonapplication-fiddler-request.PNG)

正常情况下，这个请求将返回如下的结果

![](images/daemonapplication-fiddler-response.PNG)

请复制得到的这个access_token的值。请注意，默认情况下，这个access_token会在1个小时后过期。至于怎么刷新token，我会在后续文章中介绍。

## 使用令牌访问资源

有了这个access_token，应用程序就可以尽情地访问Microsoft Graph的资源了。例如，通过下面的请求可以获取到对应的Office 365 Tenant中的所有用户信息。

![](images/daemonapplication-getusers.PNG)

## 使用一个控制台程序来实现代码逻辑

上面演示的时候，我用了Fiddler这个小工具来模拟发起请求，并且快速地查看到结果。下面用一个简单的应用程序，来实现代码逻辑，给大家参考。
>这个程序使用了最简单的代码实现，并添加了Newtonsoft.Json这个Package

```

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
```


