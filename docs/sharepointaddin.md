# SharePoint Online Add-in 开发简介
> 作者：陈希章 发表于 2017年12月22日

在 [再谈SharePoint大局观](sharepoint.md) 中我提到了SharePoint开发的一些新的变化，这一篇文章我将讲解SharePoint Add-in开发。其实早在2013年我也写过这方面的文章，有兴趣的朋友可以参考

1. [SharePoint Server 2013开发之旅（一）：新的开发平台和典型开发场景介绍](http://www.cnblogs.com/chenxizhang/p/3394652.html)
1. [SharePoint Server 2013开发之旅（二）：使用在线的开发人员网站进行SharePoint App开发](http://www.cnblogs.com/chenxizhang/p/3395046.html)
1. [SharePoint Server 2013开发之旅（三）：为SharePoint Server配置App开发、部署、管理环境](http://www.cnblogs.com/chenxizhang/p/3397543.html)

今天再回过去看这些文章，绝大部分的知识还是可以利用的。只不过，第二篇中提到的Napa这个在线的工具现在不再提供了。现在的开发工具主要有Visual Studio 和 Visual Studio Code。

> 值得一提的是，现在确定下来的SharePoint Add-in对应的是2013年那会儿说的SharePoint App，而现在SharePoint App是另有所指。我知道你已经晕掉了，但我希望大家不用纠结这个名字的问题，或者你可以简单地将他们理解为一样也没有关系。

## SharePoint Add-in开发概述

作为SharePoint Add-in的定位来说，它是用来给SharePoint提供扩展性的功能的。例如增加一个WebPart，列表，或者自定义一个工作流等等，你的Add-in也可以是一个完全独立的应用，其中会调用到SharePoint的API去完成某种程度的集成。

我知道很多人会觉得SharePoint其实很强大了，难道还有什么需要我们去扩展它的吗？这个嘛，取决于你是否真的有深度使用SharePoint吧。我们可以看一下目前已经发布到Office Store中的SharePoint Add-in有哪一些吧。目前有1207个Add-in

![](images/2017-12-22-13-37-09.png)

关于SharePoint Add-in的详细介绍，有兴趣可以参考 <https://docs.microsoft.com/zh-cn/sharepoint/dev/sp-add-ins/sharepoint-add-ins>，我这里给大家指出的是：SharePoint Add-in分为两类：

1. SharePoint自己托管的，应用的内容（网页，脚本）直接托管在SharePoint里面，无需自己创建网站。
1. 提供商托管的，应用是一个独立的网站，可以通过自己喜欢的方式进行部署。

他们的区别如下

![](images/2017-12-22-13-47-13.png)

本文的最后会通过实例快速地分别创建这两种Add-in。

## 安装开发环境

开发SharePoint Online的Add-in，只需要在客户端机器上安装开发工具即可，无需再安装服务器组件。开发工具我最推荐还是Visual Studio 2017。

Visual Studio 2017中内置了Office 365相关的开发工具

![](images/2017-12-22-13-51-36.png)

完成安装后，你可以创建项目时直接使用相关的模板

![](images/2017-12-22-13-50-00.png)

## 创建SharePoint Developer Site（开发者站点）

上面提到了本地开发工具的安装，为了便于你的开发和部署测试工作。你还需要在SharePoint Online上面创建一个开发站点。为了进行这个操作，你需要具有Office 365 全局管理员或者SharePoint Online管理员的权限。

> 如果你还没有Office 365的环境，请参考 [这篇文章](office365devenv.md) 的说明申请一个为期一年的开发者环境。

你需要登陆到SharePoint 管理中心，创建一个“私人网站集”

![](images/2017-12-22-13-57-49.png)

这个网站创建后大致是下面这样的

![](images/2017-12-22-15-19-52.png)

> 请注意，开发者网站可以创建多个。

## 创建SharePoint App Catalog Site（应用程序目录站点)

如果你有了开发者网站，你可以自己进行开发、测试和调试。但如果你真的需要在公司内部让其他同事也能使用你开发的Add-in，则需要将你的成果发布到SharePoint Online。我们可以通过创建应用程序目录站点来实现这个需求。但是，应用程序目录站点的创建不同于一般的站点，而且它在一个Office 365的租户中只能创建一个。

![](images/2017-12-22-14-17-46.png)

如果是第一次操作，则可以进入下面的界面

![](images/2017-12-22-14-22-08.png)

点击“确定”后进入详细配置页面

![](images/2017-12-22-14-23-37.png)

这个网站看起来是下面这样的

![](images/2017-12-22-14-34-50.png)

## 创建、测试和部署SharePoint-hosted Add-in

万事俱备，现在就可以开始SharePoint Online的Add-in的开发了。

第一步，通过Visual Studio创建项目

![](images/2017-12-22-14-37-42.png)

第二步，输入开发站点的路径，并且选择Add-in的类型

![](images/2017-12-22-14-38-22.png)

点击“下一步”时需要进行身份认证，请提供有权限登录到开发站点的用户名和密码，然后选择“SharePoint Online”

![](images/2017-12-22-14-40-35.png)

点击“完成”，这个向导会生成项目的结构和内容

![](images/2017-12-22-14-42-22.png)

这个项目使用了ASP.NET的技术来实现网页(default.aspx)，并且有一个app.js文件动态读取当前SharePoint的用户名，并且显示在页面上。

![](images/2017-12-22-14-44-07.png)

为了进行部署，项目中定义了一个功能（Feature），并且将其打包在一个包（Package）里面。

![](images/2017-12-22-14-45-20.png)

作为演示目的，我这里不做任何代码修改，选择项目文件夹，在右键菜单中选择“部署”，你可能会被要求再次输入账号和密码。接下来请留意在输出窗口的动态

```
------ 已启动生成: 项目: SharePointAddInSample-SelfHosted, 配置: Debug Any CPU ------
------ 已启动部署: 项目: SharePointAddInSample-SelfHosted, 配置: Debug Any CPU ------
活动部署配置: Deploy SharePoint Add-in
  由于未指定预先部署命令，将跳过部署步骤。
  正在跳过卸载步骤，因为服务器上未安装 SharePoint 外接程序。
  安装 SharePoint 外接程序:
  正在上载 SharePoint 外接程序...
  正在进行安装(00:00:05)
  外接程序已安装在 https://office365devlabs-1be72383c7171f.sharepoint.com/SharePointAddInSample-SelfHosted/ 中。
  已成功安装 SharePoint 外接程序。
  由于未指定后期部署命令，将跳过部署步骤。
========== 生成: 成功或最新 1 个，失败 0 个，跳过 0 个 ==========
========== 部署: 成功 1 个，失败 0 个，跳过 0 个 ==========

```

我们可以直接点击上面提到的一个地址，查看一下这个Add-in运行的效果

![](images/2017-12-22-15-06-54.png)

那么，怎么样在我们的站点中使用这个Add-in呢？首先，你需要先对当前这个项目进行打包。

请在项目文件夹上面点击邮件，选择“发布”，然后点击 “打包外接程序”

![](images/2017-12-22-15-08-53.png)

Visual Studio会在磁盘上面生成一个APP文件

![](images/2017-12-22-15-09-47.png)

接下来你可以将这个文件上传到开发者网站进行测试

![](images/2017-12-22-15-21-01.png)

上传文件后点击“确定”

![](images/2017-12-22-15-21-41.png)

点击“信任它”，然后需要等待一两分钟后，在网站的左侧导航区域中会出现刚才这个应用

![](images/2017-12-22-15-27-05.png)

点击“SharePointAddinSample-SelfHosted”即可运行这个应用

![](images/2017-12-22-15-27-54.png)

如果我们希望在其他网站也能使用这个应用，则需要先把这个应用发布到“应用程序目录网站”中去。在左侧选择“适用于SharePoint的应用程序”，然后点击 “上载”

![](images/2017-12-22-15-36-54.png)

然后你的网站“添加应用程序”的时候，就能看到这个自定义应用

![](images/2017-12-22-15-43-37.png)

然后选择“信任它”

![](images/2017-12-22-15-44-26.png)

稍等片刻后，你也会在左侧导航栏中看到一个新的应用链接（这个链接的文字你还可以点击“编辑链接”进行修改）

![](images/2017-12-22-15-47-35.png)

点击链接后运行的效果如下

![](images/2017-12-22-15-48-23.png)

到这里为止，我们就全部完成了一个最简单的“SharePoint托管Add-in”的开发和部署过程。你可能会说，好像看起来界面不太美观嘛，功能也太单一了（只是先试一下当前用户名）等等，确实是这样，但作为一个入门教程我相信这已经够了。

要知道，你在这个项目中还可以添加很多东西

![](images/2017-12-22-15-50-45.png)

唯一需要注意的是，这里的编程都是基于HTML和JavaScript的，不能使用服务器代码（例如C#)和服务器对象模型。

## 创建、测试和部署部署Provider-hosted Add-in

下面再给大家快速介绍一下“提供商托管的Add-in”的开发过程吧。首先，你在创建项目的时候选择“提供商托管”

![](images/2017-12-22-15-52-57.png)

目标还是选择“SharePoint Online”

![](images/2017-12-22-15-53-39.png)

然后，选择你要创建的Web应用项目的类型，推荐选择“ASP.NET MVC”

![](images/2017-12-22-15-54-44.png)

配置身份验证选项，推荐用第一种

![](images/2017-12-22-15-55-32.png)

创建好项目后，请注意这个解决方案中有两个项目了。第一个是SharePoint Add-in项目，第二个是外部那个网站的项目。

![](images/2017-12-22-16-04-36.png)

我同样不做任何代码修改直接进行部署的尝试。因为这个网站应用是所谓的提供商托管，所以我需要自己去部署。我这里选择的是Azure提供的PaaS服务来实现。

![](images/2017-12-22-16-00-38.png)

点击“创建”按钮，Visual Studio会直接帮我部署

```
1>------ 已启动生成: 项目: SharePointAddIn2Web, 配置: Release Any CPU ------
1>  SharePointAddIn2Web -> C:\Users\xxxx\source\repos\SharePointAddIn2\SharePointAddIn2Web\bin\SharePointAddIn2Web.dll
2>------ 发布已启动: 项目: SharePointAddIn2Web, 配置: Release Any CPU ------
2>已使用 C:\Users\xxxx\source\repos\SharePointAddIn2\SharePointAddIn2Web\Web.Release.config 将 Web.config 转换为 obj\Release\TransformWebConfig\transformed\Web.config。
2>已将自动 ConnectionString Views\Web.config 转换为 obj\Release\CSAutoParameterize\transformed\Views\Web.config。
2>已将自动 ConnectionString obj\Release\TransformWebConfig\transformed\Web.config 转换为 obj\Release\CSAutoParameterize\transformed\Web.config。
2>正在将所有文件都复制到以下临时位置以进行打包/发布:
2>obj\Release\Package\PackageTmp。
2>启动 Web Deploy 以将应用程序/包发布到 https://sharepointaddinsample.scm.azurewebsites.net/msdeploy.axd?site=SharePointAddInSample...

2>发布成功。
2>Web 应用已成功发布 http://sharepointaddinsample.azurewebsites.net/
========== 生成: 成功 1 个，失败 0 个，最新 0 个，跳过 0 个 ==========
========== 发布: 成功 1 个，失败 0 个，跳过 0 个 ==========
```

接下来，有一个特殊的部署，因为你的这个网站是一个外部的，为了得到授权，你需要按照下面的说明注册一个客户端ID和密钥。

[注册 SharePoint 加载项 2013](https://docs.microsoft.com/zh-cn/sharepoint/dev/sp-add-ins/register-sharepoint-add-ins)

我已经生成一个信息如下

![](images/2017-12-22-16-14-48.png)

回到Visual Studio中，修改Web.config文件和AppManifest.xml文件，然后选择SharePoint Add-in这个项目文件夹，在右键菜单中选择“发布”

![](images/2017-12-22-16-16-52.png)

点击“编辑”，然后输入刚才注册得到的信息

![](images/2017-12-22-16-15-25.png)

点击“完成”后回到主界面，点击“打包外接程序”，请注意这里将Url改为https开头

![](images/2017-12-22-16-17-59.png)

如果一切顺利的话，我们将得到一个APP文件

![](images/2017-12-22-16-18-49.png)

安装成功后你会在左侧导航栏看到一个新的链接，点击之后会跳转到这个页面

![](images/2017-12-22-16-23-05.png)

然后，后面的操作就是在你的这个自定义网站上面进行了。
