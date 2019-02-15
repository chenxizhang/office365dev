# 课程笔记 —— Microsoft Graph应用开发实战

> 陈希章 于 2019-2-10

## .NET Core 模板库

1. .NET Core 模板库 <https://github.com/chenxizhang/dotnetcore-office365dev-templates-msgraph>
1. 控制台程序
    > 安装 `dotnet new -i chenxizhang.dotnetcore.msgraph.console.CSharp` 然后运行下面命令创建项目
    * 国际版 `dotnet new graphconsole -o graphconsoleapp`
    * 国内版 `dotnet new graphconsole --instance gallatin --clientid d430823c-5613-4b1a-8d39-78f29c549f3e -o graphconsoleapp-gallatin` ，请注意，这个clientid是我创建在自己的租户里面的，你需要自己注册一个，并且替换掉它，否则可能无法使用
    > 运行 `dotnet run`
1. 网站
    > 安装 `dotnet new -i chenxizhang.dotnetcore.msgraph.web.CSharp` 然后运行下面命令创建项目
    * 国际版 `dotnet new graphwebapp -o graphwebapp`
    * 国内版 `dotnet new graphwebapp --instance gallatin --clientid 61f5f917-54fa-440f-9030-2fd8a8d2280b --secret "S8wL3NBGKiL4r30XaBwIJPvX4v+PrpqJxKLMh9/Itmk=" -o graphwebapp-gallatin` ，请注意，这个clientid和secret是我创建在自己的租户里面的，你需要自己注册一个，并且替换掉它，否则可能无法使用
    > 运行 `dotnet run`
1. Web API （on-behalf-of) -- 仅支持单租户模式
    > 安装 `dotnet new -i chenxizhang.dotnetcore.msgraph.webapi.CSharp` 然后运行下面命令创建项目
    * 国内版 `dotnet new graphwebapi --instance gallatin --clientid ef15b8d4-2762-4516-99fa-e44b9a8746f5 --secret "2TV1LxjheL2gf19tCdGO4yna0x65pJyeLbovY0PqTCE=" --tenantid "12c0cdab-3c40-4e86-80b9-3e6f98d2d344" --obo-console-clientid "5ba82619-4604-4b1b-9a73-86cd1796cbaf" -o graphwebapisample-gallatin` ，请注意，这个clientid和secret是我创建在自己的租户里面的，你需要自己注册一个，并且替换掉它，否则可能无法使用
    > 运行 `dotnet run`