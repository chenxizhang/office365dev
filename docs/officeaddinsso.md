# 在Office Add-in中实现单点登陆（SSO）
> 作者：陈希章 发表于 2017年12月27日

这篇文章经过多次修改，终于在今天晚上写完了，演示用的范例代码也终于跑通了。因为这个SSO的功能目前只是Preview的状态，所以本篇文章严格参考了官方的文档，并且对其中的重点环节做了提示，对最终效果做了说明。

> 官方的文档请参考 <https://docs.microsoft.com/en-us/office/dev/add-ins/develop/sso-in-office-add-ins> 

## Office Add-in SSO 认证流程和原理

实现Office Add-in 的单点登录是要解决一个典型问题：已登陆到Office的用户，能否直接利用他自己的身份去访问到Microsoft Graph的资源，而无需再次登录。

> 请注意，目前这个功能是Preview的状态。所以要进行测试的话，你必须是某个Office 365 Tenant的全局管理员。如果不具备这个条件，你将无法完成这个实验，但你可以通过这个文章对单点登录流程和效果有一个基本认识。

下图是Office Add-in中进行SSO认证的标准流程。这里面涉及到几个部分

1. Office 宿主程序。
1. Office Add-in 
1. Office Add-in Service（通常是一个API服务）
1. Azure AD V2（请注意，必须是2.0）
1. Microsoft Graph

![](images/2017-12-27-11-06-55.png)

基本流程如下

1. Office Add-in发起一个请求，getAccessTokenAsync。
1. Office 宿主程序会向Azure AD 发起一个请求，去获取当前用户的一个所谓的addin token。
1. Office Add-in得到这个addin token后，将其发送到Office Add-in Service，并请求Microsoft Graph的资源。请注意，Office Add-in 和 Office Add-in service必须使用同一个域名。
1. Office Add-in Service 向Azure AD发起请求，利用addin token交换得到真正的access token。
1. Office Add-in Service 利用得到的access token去访问Microsoft Graph，并将结果返回给Office Add-in。

## 如何运行官方案例，体验Office Add-in SSO的效果

我是完全按照 [这篇文章](https://docs.microsoft.com/en-us/office/dev/add-ins/develop/create-sso-office-add-ins-aspnet)进行了实验，并且已经做出来效果。下面我将有关的步骤，尤其是一些要注意的步骤介绍一下。（该文档的部分地方存在一些小问题，我已经提报给作者了）。

1. 实验环境。你需要安装最新的Visual Studio 2017和Office 2016（版本1708或者更新）。
1. 克隆这个代码库 <https://github.com/officedev/office-add-in-aspnet-sso>。
1. 在***Completed***目录中找到 `Office-Add-in-ASPNET-SSO.sln`，双击打开这个解决方案。
1. 此时编译解决方案会报错，请运行 `Install-Package Microsoft.Identity.Client -IncludePrerelease` 命令安装一个包。再次编译，请确保没有任何错误。
1. 在 <https://apps.dev.microsoft.com/> 注册一个应用程序，这个步骤比较重要，一定要严格按照说明操作。下面是我注册的信息，请一定要确保保持一致。

    ![](images/2017-12-27-11-30-45.png)

1. 将上图中的Application ID和Application Secret信息记下来，并且将Application ID的信息替换掉这个地址中的占位符：https://login.microsoftonline.com/common/adminconsent?client_id={application_ID}&state=12345 得到 https://login.microsoftonline.com/common/adminconsent?client_id=f7e0154d-1258-42ef-9e89-9857889f4e01&state=12345。
1. 目前来说，要测试这个功能，你必须拥有一个Office 365 Tenant的全局管理员身份。在浏览器中输入上面这个地址，然后提供你的管理员身份，你会收到如下的提示，要你接受授权的请求。

    ![](images/2017-12-27-11-39-37.png)

    点击“Accept”（有时候可能要两次）后，页面会被重定向到下面的地址。请不要惊慌，因为当前我们的Office Add-in网站没有运行起来，所以确实会报告“无法访问此网站”。在这里，只要将地址栏中的tenant这个查询字符串的信息记下来即可（我的例子是：`ca2af238-5e61-4608-8622-797a954c4d65`），后面要用到。

    ![](images/2017-12-27-11-42-23.png)

1. 回到Visual Studio 2017里面来，找到 `Office-Add-in-ASPNET-SSO-WebAPI` 这个项目中的Web.config文件，在appsettings里面修改几个设置信息。

    ![](images/2017-12-27-11-46-04.png)

1. 在`Office-Add-in-ASPNET-SSO` 这个项目中，找到 `Office-Add-in-ASPNET-SSOManifest`这个文件，在最底部，修改WebApplicationInfo这个节的信息如下：

    ![](images/2017-12-27-11-48-46.png)

1. 选择`Office-Add-in-ASPNET-SSO` 这个项目，在右键菜单中选择“发布”，然后新建一个配置文件：

    ![](images/2017-12-27-11-51-03.png)

    然后点击“打包外接程序”按钮

    ![](images/2017-12-27-11-52-38.png)

    输入当前本地调试用的网址后点击“完成”，你将得到一个可以用于部署的清单文件

    ![](images/2017-12-27-11-53-52.png)

1. 利用清单文件加载该Add-in。这个Add-in是用于PowerPoint的，它的详细代码我这里就不展开了，它实现了一个简单的功能用来读取当前用户的OneDrive for Business中的文件列表信息。

    我的实验环境中，当前用户的OneDrive for Business中有如下的文件：

    ![](images/2017-12-27-11-58-01.png)

    请在Visual Studio中将“Office-Add-in-ASPNET-SSO-WebAPI”这个项目运行起来，然后通过PowerPoint Online创建一个演示文档，在菜单中选择“插入”=》“Office 加载项”，然后选择“上载我的加载项”，选择上一步骤中生成的XML文件。

    ![](images/2017-12-27-12-00-54.png)

    然后你可以在工具栏中看到一个新的按钮，点击之后可以显示一个任务面板

    ![](images/2017-12-27-12-04-19.png)

    最后，激动人心的时刻终于要到了。点击任务面板中的“Get My Files From OneDrive”按钮，你将在任务面板的下方看到OneDrive for Business中的文件列表信息。

    当然，很多时候，我也发现会遇到如下的错误。

    ![](images/2017-12-27-12-07-21.png)


## 结语

本文对于最新的Office Add-in中实现单点登录（SSO）的方案进行了讲解，并且将配套的实例进行了测试。由于当前这个功能还是非常早期的阶段，所以可能你在做实验的时候，也会遇到这样那样的问题。如果遇到，可以给我留言。我过一阵子会继续关注一下这个功能的进展，给大家更新。
