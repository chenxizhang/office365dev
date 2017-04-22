# 注册Azure AD 2.0 应用程序

[上一篇](applicationregisteration.md) 介绍了Microsoft Graph应用程序的一些概念，以及目前还比较普遍的Azure AD 1.0应用程序的注册方式。但正如我多次提到的那样，虽然目前功能还在不断完善，但Azure AD 2.0会逐渐成为主流，它有如下几个优势

1. Azure AD 2.0 应用程序，既支持访问工作或学校账号，也支持访问个人账号的资源。
2. 注册Azure AD 2.0应用程序，不需要访问目标客户的Azure AD，是在一个独立的平台注册，也就是说这种应用程序天生就是Multi Tenant模式的，有更高的复用性。
3. Azure AD 2.0应用程序的权限，是动态申请的，有利于应用程序升级，简化部署和管理。
4. Azure AD 2.0应用程序，微软提供了更加高级的开发工具支持，针对大部分开发平台都有SDK提供。

说了这么多好处，下面就能实际体验一下吧

# 创建应用程序

微软提供了一个独立的应用管理平台，请用个人账号（Microsoft Account）登录 <https://apps.dev.microsoft.com>

![](images/azuread20-portal.PNG)

点击“Add an app”

![](images/azure20-addanapp.PNG)

点击“Create application"按钮，然后在详细页面中，分别点击Generate New Password 生成密钥，点击 Add Platform添加相关的平台支持

![](images/azuread20-application-details.PNG)


# 授予权限

同样的，我们也需要为这个应用程序授予权限，Azure AD 2.0应用程序在授权这部分相对来说更加简单，而且提出了一个相对更新的概念（scope，后续实际项目开发时会讲解）。

![](images/azure20-permission.PNG)

# 其他设置
你在应用程序详细页面中还能看到其他一些可配置项，他们并不是必须的

![](images/azure20-otheroptions.PNG)