# 课程笔记 —— 三问Microsoft Graph : What, Why , How

> 陈希章 于 2019-2-10

## 为什么要设计Microsoft Graph

1. Microsoft Graph 概述 <https://docs.microsoft.com/zh-cn/graph/overview>
    >Microsoft Graph 是 Microsoft 365 中数据和智能的网关。 Microsoft Graph 提供了统一的可编程性模型，通过该模型可利用 Office 365、企业移动性 + 安全性(EMS) 和 Windows 10 中的大量数据。
1. Exchange Web Service（EWS） 托管 API 客户端应用程序入门 <https://docs.microsoft.com/zh-cn/exchange/client-developer/exchange-web-services/get-started-with-ews-managed-api-client-applications>
1. Outlook 邮件 REST API 的参考 （版本 2.0） <https://docs.microsoft.com/zh-cn/previous-versions/office/office-365-api/api/version-2.0/mail-rest-operations>
1. SharePoint REST API 引用和示例 <https://msdn.microsoft.com/zh-cn/library/office/jj860569.aspx>
1. Skype Developer Platform <https://docs.microsoft.com/zh-cn/skype-sdk/skypedeveloperplatform>

## 怎么使用Microsoft Graph

1. Graph Explorer
    * 国际版 <https://developer.microsoft.com/graph/graph-explorer/>
    * 国内版 <https://graphexplorerchina.azurewebsites.net/#>
1. Azure AD 2.0应用程序注册中心 <https://apps.dev.microsoft.com/>
1. PostMan <https://www.getpostman.com/>
1. Fiddler <https://www.telerik.com/fiddler>

## 身份认证SDK

1. 获取访问令牌以调用 Microsoft Graph <https://docs.microsoft.com/zh-cn/graph/auth-overview>
1. 代表用户获取访问权限 <https://docs.microsoft.com/zh-cn/graph/auth-v2-user>
1. 在没有用户的情况下获取访问权限 <https://docs.microsoft.com/zh-cn/graph/auth-v2-service>
1. Azure AD 1.0 认证库 <https://docs.microsoft.com/zh-cn/azure/active-directory/develop/active-directory-authentication-libraries>
1. Azure AD 2.0 认证库 <https://docs.microsoft.com/zh-cn/azure/active-directory/develop/reference-v2-libraries>

## Microsoft Graph SDKs

1. Microsoft Graph API 参考 <https://docs.microsoft.com/zh-cn/graph/api/overview?view=graph-rest-1.0>
1. Microsoft Graph 入门 <https://developer.microsoft.com/zh-CN/graph/get-started>
1. Microsoft Graph GitHub库<https://github.com/microsoftgraph>
1. Microsoft Graph Stackoverflow <https://stackoverflow.com/questions/tagged/microsoft-graph>
1. 提交功能需求 <https://officespdev.uservoice.com/forums/224641-feature-requests-and-feedback/category/101632-microsoft-graph-o365-rest-apis>

## .NET Core 模板库

1. .NET Core 模板库 <https://github.com/chenxizhang/dotnetcore-office365dev-templates-msgraph>
    > 安装 `dotnet new -i chenxizhang.dotnetcore.msgraph.console.CSharp` 然后运行下面命令创建项目
    * 国际版 `dotnet new graphconsole -o graphconsoleapp`
    * 国内版 `dotnet new graphconsole --instance gallatin --clientid d430823c-5613-4b1a-8d39-78f29c549f3e -o graphconsoleapp-gallatin` ，请注意，这个clientid是我创建在自己的租户里面的，你需要自己注册一个，并且替换掉它，否则可能无法使用
    > 运行 `dotnet run`