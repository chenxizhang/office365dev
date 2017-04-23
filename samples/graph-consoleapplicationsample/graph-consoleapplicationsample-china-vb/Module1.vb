Imports System.Net
Imports Microsoft.IdentityModel.Clients.ActiveDirectory

Module Module1

    Sub Main()
        '获得用户令牌
        Dim token = GetAccessToken().Result
        '获得当前用户基本信息
        Dim user = InvokeRestRequest("https://microsoftgraph.chinacloudapi.cn/v1.0/me", token).Result
        Console.WriteLine(user)
        '获得用户的邮件列表（前十封）
        Dim messages = InvokeRestRequest("https://microsoftgraph.chinacloudapi.cn/v1.0/me/messages", token).Result
        Console.WriteLine(messages)

        Console.Read()


    End Sub

    Async Function InvokeRestRequest(url As String, token As String) As Task(Of String)
        Dim client = New WebClient()
        client.Headers.Add("Authorization", $"Bearer {token}")
        Dim result = Await client.DownloadStringTaskAsync(url)
        Return result
        '请注意，这里直接返回字符串型的结果，它是Json格式的，有兴趣的可以继续在这个基础上进行处理
    End Function

    Async Function GetAccessToken() As Task(Of String)
        Dim appId = "9c7dd51c-072c-4aea-aaee-fc57efacb150"
        Dim authorizationEndpoint = "https://login.chinacloudapi.cn/common/oauth2/authorize"
        '国际版是https://login.microsoftonline.com/common/oauth2/authorize
        Dim resource = "https://microsoftgraph.chinacloudapi.cn" '国际版是https://graph.microsoft.com
        Dim redirectUri = "http://nativeapplication" '其实这个应该去掉，目前必须要填，而且要跟注册时一样

        Dim result As AuthenticationResult
        Dim context = New AuthenticationContext(authorizationEndpoint)
        result = Await context.AcquireTokenAsync(resource, appId, New Uri(redirectUri), New PlatformParameters(PromptBehavior.Auto))

        Return result.AccessToken

    End Function


End Module
