Imports System.Net.Http
Imports Microsoft.Graph
Imports Microsoft.Identity.Client
''' <summary>
''' 说明：这个范例是介绍如何在Console Application中调用Microsoft Graph，获取用户基本信息，现有邮件列表，以及发送邮件
''' 作者：陈希章 Ares Chen
''' 时间：2017年4月4日
''' 参考：https : //github.com/chenxizhang/office365dev 
''' 必备条件：Install-Package Microsoft.Identity.Client -IncludePrerelease
''' 必备条件：Install-Package Microsoft.Graph
''' 请注意：安装完之后， 需要更新一下Newtonsoft.Json这个Package， 并且版本不能超过10.0
''' </summary>
Module Module1


    Sub Main()

        Dim serviceClient = New GraphServiceClient(New GraphAuthenticator())
        Dim user = serviceClient.Me.Request.GetAsync().Result

        '获取用户基本信息

        Console.WriteLine(user.DisplayName)
        Console.WriteLine(user.Mail)

        '获取用户的邮件列表
        Dim messages = serviceClient.Me.MailFolders.Inbox.Messages.Request.GetAsync().Result
        For Each item In messages
            Console.WriteLine(item.Subject)

        Next
        '发送邮件
        serviceClient.Me.SendMail(New Message() With {
            .Subject = "调用Microsoft Graph发出的邮件（VB.NET）",
            .Body = New ItemBody() With {
                .Content = "这是一封调用了Microsoft Graph服务发出的邮件，范例参考 https://github.com/chenxizhang/office365dev",
                .ContentType = BodyType.Text
            },
            .ToRecipients = New List(Of Recipient) From {
                New Recipient() With {.EmailAddress = New EmailAddress() With {.Address = "ares@office365devlabs.onmicrosoft.com"}}
            }
        }, True).Request.PostAsync()




        Console.Read()


    End Sub





    Public Class GraphAuthenticator
        Implements IAuthenticationProvider

        Shared token As String
        Shared Expiration As DateTimeOffset




        Public Async Function AuthenticateRequestAsync(request As HttpRequestMessage) As Task Implements IAuthenticationProvider.AuthenticateRequestAsync

            Dim clientID As String = "1e545fe0-3d58-4aa8-bcfe-972930be72dc" '这个ID是我创建的一个临时App的ID，请替换为自己的
            Dim scopes As String() = {"user.read", "mail.read", "mail.send"}
            Dim app As PublicClientApplication = New PublicClientApplication(clientID)
            Dim result As AuthenticationResult

            Try
                result = Await app.AcquireTokenSilentAsync(scopes)
                token = result.Token


            Catch ex As Exception
                If (String.IsNullOrEmpty(token) OrElse Expiration <= DateTimeOffset.UtcNow.AddMinutes(5)) Then

                    result = app.AcquireTokenAsync(scopes).Result
                    Expiration = result.ExpiresOn
                    token = result.Token
                End If
            End Try


            request.Headers.Add("Authorization", $"Bearer {token}")


        End Function
    End Class
End Module
