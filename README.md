# Office 365 开发概览系列文章和教程

## 引子

![](https://media.licdn.com/mpr/mpr/AAEAAQAAAAAAAAy_AAAAJGQ0YTJhYWM4LTYyYmYtNGMwNy1hYTQxLTdkOTgwNzM5Y2Q3Mw.jpg)

之前我在Office 365技术社群（O萌）中跟大家提到，3月初适逢Visual Studio 2017隆重发布以及20周年纪念，我想要发起一次与Office 365开发相关的活动，一方面也是向Visual Studio致敬；另一方面，我在加入微软之前，有相当长的一段时间都是从事与Office平台定制和应用系统开发的工作（最早是做VBA的开发，后来有做过VSTO，以及SharePoint开发），而以微软员工的身份跟很多客户以及合作伙伴打交道下来，我有一个深切的体会就是说Office 365这个平台不光是给客户带来了全新的体验、也给独立软件开发商（ISV）和广大的开发人员提供了前所未有的一些机会，但是这些潜力还没有完全地开发出来，我觉得有必要也有兴趣做一点这方面的分享，除了对我自己来说也是一个总结和思考的机会，如果还有幸对大家有所帮助，我将感到非常高兴。

我并不认为Office 365的平台就已经是完美的，事实上因为在全球有多个版本导致有些功能或接口并不完全统一，而且由于开发的技术（Web为主，结合了云的架构）与传统的Office开发有较大的差异，这些会给我们的开发人员带来一些困扰和挑战。我们在去年10月份的时候，在北京举办了第一届Office 365技术峰会暨开发马拉松大会，总部产品组来了将近20人的豪华团队，与中国的开发团队（或独立开发个人）做了深入广泛的交流，我们能看到大家的积极热情，也收到了不少实际的反馈意见。

> 据我所知，同类活动今年仍然将择期举办，规模和深度可能都会有提升，敬请大家关注，并且可以早做些准备。 

![](https://media.licdn.com/mpr/mpr/AAEAAQAAAAAAAA1MAAAAJGJmODgwYjEyLTUwNDEtNDQzMy05NGRiLTU5MmYyNTJjMTllYQ.jpg)

但是，如果放长远一些眼光来看，Office 365目前采用敏捷开发模式，产品组也更加开放，我相信它本身会越来越完善；与此同时，作为一个全新的平台和全新的生态，我们也希望有更多的开发人员加入，并且做出自己的贡献。

## 内容介绍

言归正传，我准备用一个系列文章和配套课程（含代码示例）的形式进行分享。这个系列将包括目前Office 365所支持的开发模式的全面介绍，并且通过案例带领观众进行实践。目前初步规划将包括如下几个内容

1. Office 365开发概述及生态环境介绍
2. Office 365开发环境搭建
3. Microsoft Graph 简介
4. Office 365 Add-in开发（Outlook）
5. Office 365 Add-in开发（Word）
6. Office 365 Add-in开发（Excel）
7. Office 365 Add-in开发（PowerPoint）
8. Office 365 Add-in开发（OneNote）
9. Office 365 Add-in开发（SharePoint）
10. SharePoint Framework开发
11. Skype for Business开发入门
12. PowerApps & Flow 实战入门
13. PowerApps & Flow 定制开发
14. Power BI 快速入门
15. Power BI 定制开发
16. Teams 应用扩展（Tab，Connector，Bot）

## 重要说明
- 需要特别声明一下，我将主要利用业余的时间来分享这个系列，所以更新的周期可能不是很有规律，但我想争取在6月底之前完整所有这个系列（包括文章和视频），如果有合适的朋友愿意参与这个系列，我非常欢迎，请私下跟我联系讨论。
- 本系列文章和课程将主要以国际版Office 365 企业版E3 作为演示和开发环境，采用的开发工具是Visual Studio 2017 Enterprise。（如果有兴趣跟随一起做练习，请预先准备好相关环境，都可以申请试用版）。
- 本系列文章将在LinkedIn和博客园、Github三个平台同步连载，欢迎大家根据自己的喜好进行关注，并且进行交流。
- 本课程将在LinkedIn Learning上面进行首发，地址请关注后期通知，并且可能会同步到有关的视频网站。
- 本系列文章和课程所涉及到的案例代码，将全部在Github上面共享，欢迎大家参与。
