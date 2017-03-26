# Office 365开发环境概览

>本文于2017年3月26日首发于LinkedIn，原文链接请参考[这里](http://www.linkedin.com/pulse/office-365%E5%BC%80%E5%8F%91%E7%8E%AF%E5%A2%83%E6%A6%82%E8%A7%88-%E5%B8%8C%E7%AB%A0-%E9%99%88?published=t)

本系列文章已经按照既定计划在每周更新，此前的几篇文章如下

1. [Office 365 开发概览系列文章和教程](http://www.linkedin.com/pulse/office-365-%E5%BC%80%E5%8F%91%E6%A6%82%E8%A7%88%E7%B3%BB%E5%88%97%E6%96%87%E7%AB%A0%E5%92%8C%E6%95%99%E7%A8%8B-%E5%B8%8C%E7%AB%A0-%E9%99%88)
1. [Office 365开发概述及生态环境介绍（一）](http://www.linkedin.com/pulse/office-365%E5%BC%80%E5%8F%91%E6%A6%82%E8%BF%B0%E5%8F%8A%E7%94%9F%E6%80%81%E7%8E%AF%E5%A2%83%E4%BB%8B%E7%BB%8D%E4%B8%80-%E5%B8%8C%E7%AB%A0-%E9%99%88)
1. [Office 365开发概述及生态环境介绍（二）](http://www.linkedin.com/pulse/office-365%E5%BC%80%E5%8F%91%E6%A6%82%E8%BF%B0%E5%8F%8A%E7%94%9F%E6%80%81%E7%8E%AF%E5%A2%83%E4%BB%8B%E7%BB%8D%E4%BA%8C-%E5%B8%8C%E7%AB%A0-%E9%99%88)

前面做了这么多铺垫，这一周终于要撸起袖子开干了。我将介绍如何搭建Office 365开发环境，本文将包括如下的两方面内容

1. 申请Office 365一年免费的开发者账号
1. 客户端开发环境介绍（Visual Studio Community，Code，Nodejs等）

## 申请Office 365一年免费的开发者账号

要进行Office 365开发，当然需要有完整的Office 365环境才可以。为了便于广大开发人员快速地启动这项工作，微软官方给所有开发人员提供了免费的一年开发者账号，申请地址如下 https://dev.office.com/devprogram
![](https://media.licdn.com/mpr/mpr/AAEAAQAAAAAAAAzDAAAAJGUwM2JjZWYyLWE4NmQtNDI2MC05M2FjLTJhNGM0MGNlYTY3OA.png)

申请的时候，需要提交一定的信息，然后很快会收到一个确认邮件，里面有一个注册链接（带有优惠码），请注意将这个链接在浏览器的私有模式下（Private Browser）打开，然后按照提示设置账号，如果不出意外的话，五分钟内，你将拥有一个完整的Office 365的环境。

> 这里建议链接在浏览器的私有模式下打开，是考虑到有不少朋友可能已经有正式在用的Office 365账号，如果不是在私有模式下，则会提示说账号已经存在，不能重复申请之类的信息。如果你有Office 365的账号，理论上已经可以开始开发，但本教程将要求大家申请这个一年的免费开发者账号，以获得一致性的体验。

![](https://media.licdn.com/mpr/mpr/AAEAAQAAAAAAAAsEAAAAJDczNzM0MDQyLTBmYTctNGVmMC04YjNlLTIwYzMxNTQ2MmU0OQ.png)
![](https://media.licdn.com/mpr/mpr/AAEAAQAAAAAAAAuFAAAAJDI2M2ZlNGUzLTc0YmMtNDFmNi04Yjc5LWQ0NTBkMjU0NzYxMw.png)

值得注意的是，申请下来的这个环境其实带有5个Office 365 E3 Developer的License，也就是说，你甚至还可以邀请四位同事（或者朋友）组成一个团队进行开发测试。

> 请注意，这里申请的是国际版的Office 365 E3，绝大部分功能在国内版的Office 365 E3中是一致的，后续文章中，如果有涉及到功能不一致的地方，我会有所说明。

![](https://media.licdn.com/mpr/mpr/AAEAAQAAAAAAAAz4AAAAJGUyY2YxZTljLWY5Y2YtNDgwYS05OWEzLWM5ZmJhMGMwODkwOA.png)

正如你所看到的那样，有了Office 365的账号，你就同时拥有了在多个设备安装Office 365 ProPlus的权利，同时还有很多有意思的服务。请自行安装，后续开发期间我们将不再对此进行赘述。

如果你对Office 365还不是很熟悉，欢迎通过下面的地址快速进行学习

https://support.office.com/learn/office365-for-business?ui=en-US&rs=en-US&ad=U



## 客户端开发环境介绍

介绍完了服务端的环境（Office 365），接下来给大家说明一下客户端开发的环境。

本系列文章将基于如下两个主要的开发环境进行介绍

1. Visual Studio 2017 Community (免费）
2. Visual Studio Code （免费，跨平台支持）

这个月初发布的Visual Studio 2017 家族，一如既往的豪华阵营，有Enterprise，Professional，Community这主要三个版本。值得注意的是，Community这个版本是免费的，而Office 365的开发是完全受Community版本支持的。
![](https://media.licdn.com/mpr/mpr/AAEAAQAAAAAAAAzpAAAAJDQ3NDY3YTNhLTY4MmYtNGI2NS04N2E1LTRlZDJlOWNlNWEyNQ.png)

在Viusal Studio 2017中开发Office 365应用，一个明显的感觉就是方便，模板和向导做的非常到位，开发人员可以将主要的精力花在业务功能上面。
![](https://media.licdn.com/mpr/mpr/AAEAAQAAAAAAAAxSAAAAJDQ2OTcxN2I5LTJjNjAtNDEwMy04NjU2LWZkMzYxMjgxMTgzMw.png)

另外，我要特别介绍的还有一个跨平台的免费开发工具，Visual Studio Code。所谓跨平台，是因为这个特殊的Visual Studio不仅仅可以在Windows上面运行，还可以在Mac，Linux上面运行，同时他能很好地支持开源的开发平台，例如NodeJS。

Visual Studio Code的下载地址是：http://code.visualstudio.com/
![](https://media.licdn.com/mpr/mpr/AAEAAQAAAAAAAAwpAAAAJDQ4OTg2NTdiLTYyMGEtNDRkOS1hYzcxLTE0OTgyMGNiNjY0OA.png)

我必须告诉你，Visual Studio Code是我非常喜欢的一款编辑器，而它对于Office 365 Add-ins开发的支持，让我感到非常意外的惊喜。在进行后续文章的学习之前，你需要先对Visual Studio Code的Office Add-in开发支持有基本的了解，http://code.visualstudio.com/docs/other/office

综上所述，我这里列出来所有客户端需要的软件环境

1. Windows 10 企业版或者专业版，推荐64位
2. Office 365 ProPlus完整安装
3. Visual Studio Community 完整安装
4. Visual Studio Code
5. Node ( 安装一些配套的npm模块，例如yo,gulp等）

值得一说的是，以我的经验，如果你能单独有一台开发的机器，将是更加理想的。我自己的做法是，在Azure里面申请了一台虚拟机来做本系列文章相关的演示代码的开发和调试。当然，如果你没有Azure的资源，那么在本地使用Hyper-V或者其他类似技术来实现也是可以的。

> Azure提供了一个Visual Studio Community 2017 on Windows 10 Enterprise的虚拟机模板，可以极大地方便开发人员快速搭建开发环境。使用云端的虚拟机的一个好处是随时随地都可以访问到它，当然这会有一定的费用，为了避免过高的费用，你可以只在使用的时候启动该虚拟机，用完即明确停止。