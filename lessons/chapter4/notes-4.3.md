# 课程笔记 —— Office Add-in 单点登陆实现方案

> 陈希章 于 2019-2-10

1. 参考链接 <https://docs.microsoft.com/zh-cn/office/dev/add-ins/develop/create-sso-office-add-ins-aspnet>

## 本地测试完整步骤

1. 准备开发环境，Visual Studio 2017，Office 365国际版管理员账号
1. 客户代码库 <https://github.com/officedev/office-add-in-aspnet-sso>
1. 打开Completed目录下面的sln文件，并且解决编译问题 `Install-Package Microsoft.Identity.Client -Version 1.1.4-preview0002`，确保能运行
1. 注册应用程序 <https://apps.dev.microsoft.com/>, 这个步骤至关重要，必须要用Office 365管理员账号进行登陆，详细操作请参考 <https://docs.microsoft.com/zh-cn/office/dev/add-ins/develop/register-sso-add-in-aad-v2>，预分配的三个应用的信息如下，请注意记录clientid和secret

    ``` text
    d3590ed6-52b3-4102-aeff-aad2292ab01c (Microsoft Office)
    57fb890c-0dab-4253-a5e0-7188c88b2bb4 (Office Online)
    bc59ab01-8403-45c6-8796-ac3ef710b3e3 (Office Online)
    ```
    申请权限：Files.Read.All,offline_access, openid,profile,User.Read

1. 管理员授权 `https://login.microsoftonline.com/common/adminconsent?client_id=0807e168-d857-4154-a040-2cf1d8ee2bf0&state=12345`,请注意替换成你的clientid信息，完成授权后，记录tenant信息（是一个GUID）
1. 修改web.config文件

    ``` XML
    <add key="ida:ClientID" value="0807e168-d857-4154-a040-2cf1d8ee2bf0" />
    <add key="ida:Audience" value="0807e168-d857-4154-a040-2cf1d8ee2bf0" />
    <add key="ida:Password" value="kssxNDGGMZ974-tbhE51=$]" />
    <add key="ida:Issuer" value="https://login.microsoftonline.com/e80eded3-f2f5-41de-a553-f4b7332744d1/v2.0" />
    ```

1. 修改manifest.xml文件

    ``` XML
    <WebApplicationInfo>
      <Id>0807e168-d857-4154-a040-2cf1d8ee2bf0</Id>
      <Resource>api://localhost:44355/0807e168-d857-4154-a040-2cf1d8ee2bf0</Resource>
      <Scopes>
        <Scope>openid</Scope>
        <Scope>offline_access</Scope>
	    <Scope>profile</Scope>
        <Scope>files.read.all</Scope>
      </Scopes>
    </WebApplicationInfo>
    ```

1. 发布Add-in，生成可以部署的manifest文件
1. 将网站在本地运行起来
1. 在PowerPoint Online中测试这个Web Addin

## 生产部署注意事项

1. 必须确保之前的这个应用，是用Office 365 管理员账号注册的
1. 用该管理员账号登陆到 <https://portal.azure.com> ，注册一个自定义域名，证明你拥有一个域名 <https://docs.microsoft.com/en-us/azure/active-directory/fundamentals/add-custom-domain>
1. 将网站部署到某个地方后，确保用自定义域名可以访问到
1. 修改之前注册好的应用，将地址从localhost的形式改成网站实际部署的地址
