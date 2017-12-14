# 使用PowerApps快速构建基于主题的轻业务应用 —— 入门篇
> 作者：陈希章 发表于 2017年12月12日

## 前言

在上一篇文章 [基于Office 365的随需应变业务应用平台](officebusinessapp.md) 中我提到，随着随需应变的业务需要，以及技术的发展，业务应用的开发的模式也有了深刻的变化。基于微软的平台，有服务于主干业务应用的Dynamic 365 业务应用平台（包括CRM和ERP），也有服务于员工日常工作的Office 365 生产力平台。这看起来非常清晰，但他们的界限其实在逐渐模糊，谁说在Office 365上面就不能进行业务操作呢？又谁说在Dynamics 365这个成熟的平台上用户不能自己去定义自己需要的应用呢？

> 我曾经在年初的 [这篇文章](https://www.linkedin.com/pulse/风起云涌西雅图技术培训见闻分享-希章-陈/) 提到Office 365 这些年本身也在朝微服务方面发展，不仅服务粒度越来越小，易于根据用户的需求进行组合，同时还创新性提供了包括Bookings和Staffhub这种专业性的业务服务，有兴趣的朋友可以参考 [bookings](https://products.office.com/en-us/business/scheduling-and-booking-app), [staffhub](https://staffhub.ms/), 并且我们有理由相信，这只是一个开始。

![](images/businessplatformtrend.PNG)

我们今天要谈论的基于主题的轻业务应用，更多是偏向前台创新应用和差异化应用。而所谓的随需应变，就是让更多的业务人员拥有构建面向主题的业务应用的能力，并且能随时根据捕捉到的信息进行调整，以达到快速响应变化的目标。

![](images/WeChat_Image_20170908063810.jpg)

为了使得业务用户自己有能力构建基于主题的轻业务应用，微软给出的答案是一套全新定义的商业应用平台，主要包括了PowerApps，Flow，PowerBI这三个组件。他们与Office 365以及Dynamics 365是紧密的集成关系（当然，他们也支持很多其他的外部系统），通过底层的通用连接器、数据模型、网关进行连接，并且在必要的时候，也支持高级定制化。

![](images/businessplatformarc.png)

PowerApps可以根据数据模型快速生成移动优先和云优先的业务应用，这个应用里面如果需要实现业务流程，则通过Flow来解决，而最终产生的大量数据，则通过PowerBI来展现，或者根据数据的规则发起新的流程或者应用操作，它们形成了一个闭环，可以满足不断优化的、随需应变的业务需要，最重要的一个前提是，这一切都是由业务用户自己来做的，无需编程。本文将用实例介绍PowerApps的快速入门，其中包括四个场景：

1. 基于一个保存在OneDrive for Business个人网盘中的Excel文件创建业务应用
1. 基于SharePoint Online的列表创建轻业务应用
1. 基于Dynamics 365 创建自定义应用
1. 在Microsoft Teams中集成PowerApps

## 先决条件

在如下的几种情况下，你可以开始使用PowerApps

1. 你已经拥有下面的Office 365授权
    ```
    Office 365 Business Essentials 
    Office 365 Business Premium 
    Office 365 Education 
    Office 365 Education Plus 
    Office 365 Enterprise E1 
    Office 365 Enterprise E3 
    Office 365 Enterprise E5
    ```
1. 你已经拥有下面的Dynamics 365授权
    ```
    Dynamics 365 for Sales, Enterprise edition 
    Dynamics 365 for Customer Service, Enterprise edition 
    Dynamics 365 for Operations, Enterprise edition 
    Dynamics 365 for Field Service, Enterprise edition 
    Dynamics 365 for Project Service Automation, Enterprise edition 
    Dynamics 365 for Team Members, Enterprise edition 
    Dynamics 365 for Financials, Business edition 
    Dynamics 365 for Team Members, Business edition 
    ```
1. 你单独购买了PowerApps
    ```
    PowerApps Plan 1
    PowerApps Plan 2
    ```

以上不同的授权存在一定的功能差异，请参考<https://powerapps.microsoft.com/en-us/pricing/>

> 截至目前为止，以上提到的PowerApps，Flow，PowerBI，除了PowerBI之外，另外两个组件还没有在中国区部署，据产品组给的一个大致的时间表是在2018年的中期有望落地。目前在国内访问PowerApps服务，偶尔会出现速度稍慢的问题，请大家谅解。

PowerApps是给业务用户来准备的，所以他的使用并不需要你懂编程，甚至都不需要了解数据库这些细节。PowerApps默认已经附带了一些标准的范例，你可以直接体验。

![](images/powerapps-samples.png)

使用标准范例不过瘾？那就让我们用实例来练习一下吧。

## 基于一个保存在OneDrive for Business个人网盘中的Excel文件创建业务应用

让我们就从Excel开始吧。假设我们的场景是这样：你是一个销售部门主管，有一个Excel文件，是用来保存订单数据的，你希望快速开发一个轻量级的业务应用，可以让你以及同事快速地在手机上面就可以输入或修改订单信息、查询订单列表，以及其他一些你认为有意思的事情。有了PowerApps，你不需要等待开发人员（不管是你公司内部的IT部门同事，还是外面的专业团队）去开发一个网页，或者定制一个移动App，然后等待一到两周才能看到真正用起来。你要的只是把Excel文件定义好，就像下面这样

![](images/powerapps-excel-file.PNG)

> 这里有一个小技巧：尽量用英文定义标题，否则你可能会遇到一些小问题。

没有什么特别的，除了你需要定义一个表格，然后把这个文件保存在你的OneDrive for Business中即可。（你在第一行输入标题后，选择A1，然后在“插入”菜单中选择“表格”）

接下来，你要做的是打开<https://preview.web.powerapps.com>这个在线的应用开发平台，用你的账号（不管是Office 365账号，还是Dynamics 365账号，或者单独的PowerApps账号）

![](images/web.powerapps.portal.png)

点击左侧的Apps，然后选择右上角的"Create an app"

![](images/powerapps-create-apps.png)

选择“OneDrive for Business” 里面的“Phone Layout”

![](images/powerapps-onedrive-excel.png)

如果你是第一次运行，则可以通过下面的界面创建一个连接

![](images/powerapps-onedrive-createconnection.PNG)

如果你之前已经创建过连接，则定位到并单击你保存的Excel文件，PowerApps会自动检测文件内部的表格，选中其中一个表格后，点击右下角的Connect按钮

![](images/powerapps-onedrive-selectexcel.png)

一两分钟后，你就能看到一个自动生成的应用

![](images/powerapps-excel-app.png)

我们这里先不展开细节，可以直接按F5运行这个应用

![](images/powerapps-excel-1.png)

点击右上角的加号（+），可以输入订单信息

![](images/powerapps-excel-2.png)

点击右上角的勾号，可以保存当前这条记录，并且自动给回到主界面，此时会显示所有的订单列表

![](images/powerapps-excel-3.png)

如果点击某条记录，则会进入订单的详细界面

![](images/powerapps-excel-4.png)

点击右上角的笔形按钮，可以进入订单的编辑视图

![](images/powerapps-excel-2.png)

到这里为止，我们就完成了一个最简单、但确实能立即工作的轻业务应用，你已经在预览界面中进行了操作，接下来要做的是什么呢？当然是保存这个应用啦。

![](images/powerapps-excel-5.PNG)

> 正如你看到的，你还可以将这个应用保存在本地计算机（This computer）。这个操作会生成一个扩展名为msapp的文件，收到这个文件的用户也可以双击打开应用。

然后你可以将应用分享给需要的同事

![](images/powerapps-excel-6.PNG)

点击“Share this app” 按钮, 你可以一次性添加公司中所有同事，让他们可以使用这个app，也可以单独添加某个同事。后面这种情况下，你还可以授权给这位同事可以一起编辑。如下图所示

![](images/powerapps-share-app.png)

我们指定分享的同事会收到一封邮件，大致如下图所示

![](images/powerapps-share-app-email.png)

如果他点击了“Use the app”的话，会弹出下面的页面，要求他进行授权

![](images/powerapps-share-permission.png)

这里的授权，主要是希望得到用户的许可，OneDrive for Business可以代表用户去进行必要的操作。

![](images/powerapps-onedrive-permission.PNG)

但这里需要特别注意的是，如果该同事并没有被授予访问上面提到这个Excel文件的权限的话，虽然能打开OrderApp，但无法读取任何数据，也无法进行操作。作为应用的作者，我需要在OneDrive for Business中选择该文件，然后给同事授予访问权限。

![](images/powerapps-onedrive-grantpermission.PNG)

到现在为止，一个最简单但足够实用的应用就创建好了，你可以通过网页版（https://web.powerapps.com/home) 进行访问，也可以通过免费的一个Windows桌面客户端（PowerApps）来进行使用。但用得最多的场景，我估计是使用手机吧。目前PowerApps这个应用可以在Apple Store和Google Play等应用市场中免费下载。

![](images/powerapps-ios.jpg)

这个PowerApps其实相当于是一个超级App，它负责来运行我们自定义的业务应用。打开这个PowerApps，输入账号和密码登陆后，可以看到你有权使用的所有应用。

![](images/powerapps-applist.jpg)

点击某个应用，你就可以进行数据查询和操作了，这一点都不奇怪，以至于我都不想多做截图了。我这里要给大家做一个提示的是，如果某个应用你需要经常使用，你甚至可以将它固定在手机的屏幕上面，而无需每次都进入PowerApps这个主界面，然后再查找了。

![](images/powerapps-pinttohome.jpg)

最后，所有用户在PowerApps中操作的数据，都将统一保存在Excel文件中。值得注意的是，PowerApps会在表格中增加一个特殊的列：__ PowerAppsId __ 用来唯一标识每一行。

![](images/powerapps-excel-result.PNG)

以上我用了很长的篇幅，完整地介绍了如何基于OneDrive for Business中保存的一个Excel文件快速开发一个业务应用，并且分发给公司里面的同事，他们可以有多种方式进行使用的场景。接下来，我将继续展示两个最典型的场景。

## 基于SharePoint Online的列表创建轻业务应用

SharePoint 作为业界领先的团队协作和内容管理平台的能力已经得到了数以亿计的用户所认可。在团队协作这个场景中，有基于文档或者内容（如笔记）的协作，也有基于工作任务的协作。不光是文档，还是工作任务，他们的本质上都是一个列表。列表的强大超过了很多人的想象，以至于我这里并不准备过多地展开细节。我反而要说一说的是列表的简单，只要你会用Excel，你肯定会用列表，而且我鼓励你这样思考：SharePoint的列表是一种服务器技术，用来像Excel那样帮我们保存各种数据，它的共同编辑和协作更加容易。

要创建一个列表非常容易，在你的团队网站的首页上面，点击“新建”按钮，选择“列表”，然后输入一些基本信息即可

![](images/powerapps-sharepoint-createlist.png)

然后为这个列表增加一些字段，最终效果如下

![](images/powerapps-sharepoint-list.png)

细心的朋友肯定都已经发现了，在列表的顶部工具栏中，其实已经看到了PowerApps这个按钮，可以说这是PowerApps与SharePoint无缝整合的有力证明了。点击这个按钮，会有两个选项，一个是“创建应用”，一个是“自定义表单”。我们先选择第一个吧。

![](images/sharepoint-powerapps-create.png)

点击“创建”按钮，差不多一两分钟时间，PowerApps会根据SharePoint这个列表结构，自动能够生成一个应用。

![](images/sharepoint-powerapps-newapp.png)

即便不做任何修改，这个应用也已经能用来填写工作日志了。至于如何分享，如何在移动设备中使用，我这里就不再赘述。但直得注意的是，如果一个列表关联了至少一个PowerApps应用的话，它的主界面会多出来一个对应的视图

![](images/powerapps-sharepoint-list-app.png)

点击“打开”按钮将启动PowerApps对列表进行操作

![](images/powerapps-sharepoint-list-1.png)

其实PowerApps只是用户界面，所有的数据都是保存在列表里面的

![](images/powerapps-sharepoint-list-2.png)

> 不要忘记，如果你要分享给同事，希望他们能使用这个应用能提交工作日志的话，他们必须被授予访问这个列表的权限。

看起来挺方便的，不是吗？这还不是全部呢。下面介绍另外一个PowerApps与SharePoint结合的场景。我相信，虽然有了PowerApps，还是会有一些用户习惯直接在SharePoint里面编辑和修改列表数据。我们先来看一下默认情况下SharePoint提供的列表项编辑界面吧

![](images/powerapps-sharepoint-list-3.png)

这个默认的界面已经很不错了，但如果用户想要有自己的界面，我们该怎么做呢？很久很久以来，我们有一个强大的技术，叫做Infopath，它是一种基于XML定义的表单技术，使用它可以自定义SharePoint列表的界面。我以前写过很多这方面的文章，其中一篇可以参考 <http://www.cnblogs.com/chenxizhang/archive/2010/04/22/1718090.html> .

但Infopath有它的问题，而且对于SharePoint的版本有依赖。进入SharePoint Online的时代以来，我们已经不使用Infopath了。但直到现在，才揭晓了它的替代方案，那就是PowerApps。

其实很简单，在列表的工具栏中选择“PowerApps”，在下拉菜单中选择“自定义表单”，你就很快看到会生成一个应用。

![](images/powerapps-sharepoint-form.png)

请注意，为了让大家看到效果，我在界面底部故意加了一个文字。选择左上角的“Back to SharePoint”按钮，按照提示发布应用，然后在SharePoint页面上再次创建列表项的话，就会看到下面这样的界面。

![](images/powerapps-sharepoint-form-2.png)

> 请注意，这个自定义表单功能，只影响网页编辑界面。SharePoint 移动App上面的界面还是会使用默认的。


## 基于Dynamics 365 创建自定义应用

最后，我们快速来了解一下PowerApps如何跟Dynamics 365结合创建自定义应用。这个场景其实跟前面两个也很类似，无外乎是数据源换成了Dynamics 365而已吧。

![](images/powerapps-d365-1.PNG)

选择“Dynamics 365”这个模板, 然后用你的Dynamics 365账号创建一个连接，选择到合适的业务实体对象

![](images/powerapps-d365-2.png)

然后点击“Connect”，PowerApps又开始根据给定的数据结构自动生成应用的神奇工作了。每到这个时候，我就想起那个“怎么样分三步将一头大象装进冰箱”的经典桥段。

![](images/powerapps-d365-3.png)

后面的步骤与OneDrive for Business非常类似，我这里就不重复了。

## 将PowerApps应用集成到Microsoft Teams中

之前使用PowerApps的业务应用至少有三种方式：网页，桌面客户端，移动客户端。现在又多了一个选择，就是将它直接集成到Microsoft Teams这个一站式的协作和沟通工具中。

Microsoft Teams是Office 365的一个组件，如果你还不熟悉，请参考 <https://products.office.com/zh-cn/microsoft-teams/group-chat-software>

下面是我们看到的一个常见的Team的界面

![](images/powerapps-teams-1.png)

点击界面上“Wiki”旁边的加号，我们可以添加PowerApps这个功能作为一个选项卡

![](images/powerapps-teams-2.png)

如果第一次使用，会有一个界面，提示安装，你只需要点击“Install”即可，然后会进入下面的界面

![](images/powerapps-teams-4.PNG)

点击“Save”，会自动创建一个选项卡，以后用户就可以直接在Teams中运行这个应用了

![](images/powerapps-teams-5.png)


> 请注意，如果是在Microsoft Teams的移动客户端中，会尝试直接打开PowerApps应用，而不是在Teams中打开应用

## 扩展阅读

看完这一篇入门文章，我不能保证你对PowerApps有了很深的理解，但我确信你可以认识到PowerApps的威力了吧。要把这个工具用好，你可能还需要进一步的阅读，我这个系列后续还有高级篇，我还是推荐大家记住PowerApps的官方文档地址 <https://docs.microsoft.com/zh-cn/powerapps/>
