# Office 365 机器人（Bot）开发入门指南 (新篇)
> 作者：陈希章 发表于 2018年2月19日

最近在整理书稿时，发现我在2017年7月份写的这篇 [Office 365 机器人（Bot）开发入门](botframeworkquickstart.md) 的内容，因为相关平台的升级，已经完全不能体现当前的开发过程，所以我再专门写一篇新的开发入门指南给有兴趣的大家参考。

这次平台升级的具体开始时间我不得而知，但是如果你现在继续访问 <https://dev.botframework.com/> ，你将会看到一个明确的提示，要求将早先创建好的Bot在2018年3月31日之前迁移到新的Azure Bot Service：

![](images/2018-02-19-13-39-09.png)

如果你此时点击了“Create a bot”按钮的话，你也将被要求通过Azure Portal来操作：

![](images/2018-02-19-13-40-58.png)

这将意味着，现在要进行机器人的开发，你必须首先拥有一个Azure的订阅，不管是试用版的，还是正式版的。本文将假定你已经拥有这些条件。

> 值得注意的是，目前Azure的国内版还没有Bot Service的功能。

## 三种不同类型的Bot

你可以在Azure Portal中搜索Bot Service，或者快速通过 [这个地址](https://portal.azure.com/?l=zh-hans.zh-cn#blade/Microsoft_Azure_Marketplace/GalleryResultsListBlade/selectedSubMenuItemId/%7B%22menuItemId%22%3A%22gallery%2FCognitiveServices_MP%2FBotService%22%2C%22resourceGroupId%22%3A%22%22%2C%22resourceGroupLocation%22%3A%22%22%2C%22dontDiscardJourney%22%3Afalse%2C%22launchingContext%22%3A%7B%22source%22%3A%5B%22GalleryFeaturedMenuItemPart%22%5D%2C%22menuItemId%22%3A%22CognitiveServices_MP%22%2C%22subMenuItemId%22%3A%22BotService%22%7D%7D) 定位到目前支持的三种Bot Service类型，如下图所示：

![](images/2018-02-19-13-48-58.png)

它们分别的使用场景如下

1. Web App Bot。这种类型将在Azure中创建一个App Service来运行你的Bot，并且通过模板和自动化的配置极大地简化你的开发过程。
1. Function Bot。这种类型将在Azure中创建一个Azure Function App来运行你的Bot，同样也是会有模板和自动化配置来简化开发，它与Web App Bot的区别在于，它的计费是按照具体的使用次数，而不是虚拟机的启用时间——事实上，这也正是Azure Function App和web App的本质区别。我个人觉得，这种形式应该是更加符合机器人的特点的——它是按需调用的，并不见得要一直运行在后台。
1. Bot Channels Registration。这种类型是支持你将Bot应用部署到你自己选择的其他位置（可能是你的数据中心，也可以是其他的云平台），然后通过Azure来做Channel的注册和对接。如果你看过我上一篇文章，你应该会对Bot，Bot Framework，Channel有一些概念，如果还不太明白，我下面会继续做一些解释。

> 在开发阶段，不管是上述哪一种类型的Bot，我们都可以选择“免费”的价格进行开发和调试（普通信道无限量消息，高级信道每月10000次消息调用）。"免费”的服务是没有SLA保障的，但对于开发阶段来说已经足够了。

## 三种常见的Azure 机器人服务方案

虽然我们知道创建Bot并不难，本文后半部分也将再次以一个实例来介绍如何开发和测试基于Azure Bot Service的机器人。但在此之前，我还是摘录三种常见的Azure机器人服务方案给大家参考，了解这些业界流行的做法和流程，可能会对你后续开发有借鉴意义。

### 商务聊天机器人

Azure 机器人服务和语言理解服务结合可使开发人员能够创建针对各种场景的对话接口，如银行、旅游和娱乐。例如，酒店礼宾员可以使用机器人增强传统的电子邮件和电话呼叫交互，方法是通过 Azure Active Directory 验证客户，并使用认知服务更好地根据实际情景利用文字和语音处理客户请求。可以添加语音识别服务来支持语音命令。

![](images/2018-02-19-14-07-23.png)

### 信息聊天机器人

此信息机器人可回答知识集中定义的问题或使用认知服务 QnA Maker 回答常见问题，以及使用 Azure 搜索回答更加开放的问题。

![](images/2018-02-19-14-08-00.png)

### 企业效率聊天机器人

Azure 机器人服务可轻松与语言理解结合以生成强大的企业效率机器人，让组织可以通过集成外部系统（如 Office 365 日历、Dynamics CRM 中存储的客户事例等）来简化常见工作活动。

![](images/2018-02-19-14-08-30.png)

## Function Bot 开发和调试

下面我将以一个实例来演示如何开发和调试Function Bot。在下图的向导中，你需要指定一个唯一的名称，并且选择存储位置，定价层（我选择F0，是指免费的定价），宿主计划我选择的是“消耗计划”指的是按调用次数付费，Application insights选择“打开”以便后期可以通过一个仪表盘来看到机器人被调用的统计数据。

![](images/2018-02-19-14-23-44.png)

你已经看到了，Azure Bot Service默认提供了两种语言（C#和Node.js）的五种模板。我先以Basic为例创建一个应用。创建成功后，请在下面的界面中点击“Test in Web Chat”来进行测试。

![](images/2018-02-19-14-40-31.png)

### 在线修改代码并且进行测试

这就是Basic模板默认提供的功能，它就像是一个回声筒一样，将你发送过去的话再返回过来。如果你觉得这样太无聊了，你当然可以修改代码让它变得有趣一些。请点击“机器人管理”中的“内部版本”这个菜单。

![](images/2018-02-19-14-42-10.png)

点击“在Azure Functions中打开此机器人”链接，在接下来的界面中，找到EchoDialog.csx这个文件，按照下面红色框示意修改代码

![](images/2018-02-19-14-45-09.png)

点击“保存”按钮，然后回到此前的"Test in Web Chat”页面，再次输入你的消息，观察其返回的内容，现在在回复消息中多了一个时间戳了。

![](images/2018-02-19-14-47-51.png)

### 本地修改机器人代码并实现持续整合

以上演示了如何在线修改代码并进行测试的方法。只要你愿意，你随时可以将代码下载到本地，然后使用你喜欢的编辑器进行本地开发，最后提交给Azure Bot Service。请在下图中点击“下载zip文件”链接。

![](images/2018-02-19-14-51-45.png)

你需要使用Visual Studio 2017打开这个解决方案文件

![](images/2018-02-19-14-54-39.png)

将上面这一行代码稍作修改，例如：```await context.PostAsync($"{this.count++}: You said {message.Text} at {DateTime.Now},Modify by Visual Studio");```。

接下来，我们要将本地这个目录进行git配置，以便后续可以跟Azure Bot Service 进行持续整合（通过git的代码提交，自动替换Azure Bot Serivce代码并触发编译，更新Bot应用）。请确保你的本地计算机上面安装了git工具。

![](images/2018-02-19-15-07-20.png)

以上通过```git init```命令初始化当前目录的git仓库。然后通过```git add .```命令和```git commit -m```命令提交本地更新。接下来我们配置Bot Service以便它能使用本地git仓库进行持续整合。

![](images/2018-02-19-15-10-21.png)

请点击上图的“所有应用服务设置”菜单，并且接下来的“部署选项”中选择“本地Git存储库”选项

![](images/2018-02-19-15-13-43.png)

点击“保存”按钮后，设置“部署凭据”。请牢记这个用户名和密码，并且不要泄露给其他人。

![](images/2018-02-19-15-15-16.png)

在“概述”页面中，此时会多出来一个Git的克隆Url，如下图所示

![](images/2018-02-19-15-17-08.png)

请将这个地址复制下来，接下来回到git bash的窗口。通过```git remote add origin 你的url``` 命令添加远程存储库绑定，并且通过```git push origin master```命令来完成代码推送。

![](images/2018-02-19-15-19-32.png)

推送成功后稍等片刻，再次回到Azure Bot Service的“Test in Web Chat”菜单，你会发现刚才我们在Visual Studio中进行的代码修改已经起了作用，如下图所示。

![](images/2018-02-19-15-21-12.png)

### 使用Bot Framework Emulator进行调试

如果你想进行更加细节的调试，我推荐你下载和安装 [Bot Framework Emulator](https://aka.ms/bf-emulator)。通过它来进行调试的好处是可以清晰地看到消息发送和接收的细节，如下图所示

![](images/2018-02-19-15-30-42.png)

### 在你的业务应用中整合这个机器人

上面我们演示了如何开发、测试和调试机器人，默认情况下，Azure Bot Service会将这个机器人连接到一个Web Chat的信道（Channel），这样的话，我们既可以通过之前多次演示的“Test in web chat”界面进行使用，但也可以将这个界面整合到自己的业务应用中来，为此我们需要获取机器人嵌入代码，如下图所示

![](images/2018-02-19-15-37-09.png)

你可以配置多个站点，并且为每个站点都生成一个单独的密钥以进行区分，然后点击“复制”按钮，实际上你会得到一串HTML代码，里面是一个iframe。请注意用你的密钥替换掉代码中的“使用此处的密钥”，请将代码保存为一个HTML文件，如下图所示

![](images/2018-02-19-15-42-47.png)

请注意，我这里添加了一个Style的设置，这是为了让它在浏览器中看起来更加美观一些。接下来你可以在任意浏览器中打开这个本地网页，输入消息后你会得到跟此前一致的使用体验。

![](images/2018-02-19-15-43-43.png)

### 将机器人连接到Microsoft Teams

既然这篇文章讲的是“Office 365 机器人（Bot）开发入门”，自然要提到如何跟Office 365的结合。这个话题有两层含义，首先在Bot Service中可以通过Microsoft Graph调用Office 365的服务来完成一些工作，其次是我们可以将机器人连接到Office 365的组件中来，目前支持Microsoft Teams和Skype for Business两个信道，如下图所示

![](images/2018-02-19-15-49-00.png)

添加到Microsoft Teams相对容易一些，你只需要点击上图中的Microsoft Teams图标，并且接受协议，在下图中点击“完成”即可。

![](images/2018-02-19-15-50-23.png)

回到信道主界面，点击“Microsoft Teams”的链接，即可为自己的Microsoft Teams客户端添加当前这个机器人。

![](images/2018-02-19-15-51-17.png)

如果不出意外的话，你的联系人中会出现一个机器人，你可以像跟同事聊天一样与它进行互动了。

![](images/2018-02-19-16-01-13.png)

如果你的同事也需要使用这个机器人，在你没有将这个应用提交给微软官方的市场之前，他们需要通过机器人的编号进行搜索，如下图所示

![](images/2018-02-19-16-06-40.png)

添加联系人后，后续的聊天形式是一样的

![](images/2018-02-19-16-07-54.png)

关于如何将你开发的这个机器人提交到微软的官方市场，请参考 <https://docs.microsoft.com/zh-cn/microsoftteams/platform/publishing/apps-publish> 的说明。


### 将机器人连接到Skype for Business

与Microsoft Teams相比，将机器人连接到Skype for Business的体验正好相反——它的安装配置过程比较复杂（需要Office 365管理员权限），但一旦配置完成，则整个公司的用户都能直接搜索到这个机器人，而无需发布到微软的应用市场。

![](images/2018-02-19-16-11-43.png)

添加Skype for Business这个信道只是第一步，接下来要根据一个文档的说明，使用Office 365管理员身份以及几个PowerShell的命令来完成这个机器人的注册和配置。通常的指令形式如下图所示

![](images/2018-02-19-16-13-18.png)

在我的Office 365测试环境中，我执行的命令如下图所示（请注意，第二个命令的执行可能需要几分钟时间）

![](images/2018-02-19-16-26-16.png)

完成上面的配置后，任何一个用户，都可以直接在Skype for Business中搜索中这个机器人并且跟它聊天了。

![](images/2018-02-19-16-30-15.png)

请注意，我前面已经提到了，因为当前这个机器人是托管在免费的模式下，所以可能你在测试过程中偶尔会遇到一些错误。


## 结语

新年新气象，这篇文章给大家完整地介绍了全新的Azure Bot Service提供的服务。我们可以利用它快速地完成机器人的开发、测试，根据自己的需要将其连接到包括Microsoft Teams和Skype for Business在内的多个信道中去，真正实现“一次编写、处处运行”，我相信这种平台级别的创新，结合Office 365的庞大用户群体，以及海量的有价值的信息，使得机器人(Bot)不再是一个实验室中的花骨朵，而是我们现实工作和生活中可以触手可及的应用。

顺便预告一下，第三届Office 365开发者大会将于3月17-18日在北京将隆重举办，我将做关于机器人开发的主题演讲。这一届大会规模比之前两届大，精彩内容除了Office 365, 还有Windows 10的最新开发场景。欢迎踊跃报名，希望现场可以看到你。活动详情和报名请访问 <https://aka.ms/M365DevDays>, 你也可以通过扫描以下二维码进行访问

![](images/2018-02-19-16-36-15.png)

