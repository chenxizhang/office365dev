# 这个脚本演示如何通过PowerShell管理Office 365应用程序
# 作者：陈希章 《Office 365 开发入门指南》第二版  于 2019-2-9

# 准备工作，请通过单独打开一个Powershell窗口，并用管理员方式打开
# Install-Module -Name AzureAD

# 连接到Azure AD
Connect-AzureAD

# 连接到国内版Azure AD
Connect-AzureAD -AzureEnvironmentName AzureChinaCloud

# 获取所有的应用程序列表
Get-AzureADApplication

# 获取某个应用程序的详细信息
Get-AzureADApplication -ObjectId xxxxxxxxx

# 创建一个新的应用程序（客户端）
$clientapp = New-AzureADApplication -ReplyUrls "" -PublicClient $true -DisplayName "xxxx"

# 创建一个新的应用程序（网页）,需要有Homepage和IdentitierUris
# $webapp = New-AzureADApplication -DisplayName $name  -ReplyUrls $replyurl -Homepage "https://websample.com" -IdentifierUris "https://websample.com"


# 定义权限
$graphrequest = New-Object -TypeName "Microsoft.Open.AzureAD.Model.RequiredResourceAccess"
$graphrequest.ResourceAccess = New-Object -TypeName "System.Collections.Generic.List[Microsoft.Open.AzureAD.Model.ResourceAccess]"
$ids = @("024d486e-b451-40bb-833d-3e66d98c5c73", "e383f46e-2787-4529-855e-0e479a3ffac0", "e1fe6dd8-ba31-4d61-89e7-88639da4683d", "b340eb25-3456-403f-be2f-af7a0d370277")
foreach ($id in $ids) {
    $obj = New-Object -TypeName "Microsoft.Open.AzureAD.Model.ResourceAccess" -ArgumentList $id, "Scope"
    $graphrequest.ResourceAccess.Add($obj)
}
$graphrequest.ResourceAppId = "00000003-0000-0000-c000-000000000000"

# 权限绑定
Set-AzureADApplication -ObjectId $clientapp.ObjectId -RequiredResourceAccess ($graphrequest)

# 完成管理员授权
$IsGallatin = $true
if ($IsGallatin) {
    Start-Process ("https://login.chinacloudapi.cn/common/adminconsent?client_id=" + $clientapp.AppId + "&state=12345&redirect_uri=https://developer.microsoft.com/en-us/graph/")
}
else {
    Start-Process ("https://login.microsoftonline.com/common/adminconsent?client_id=" + $clientapp.AppId + "&state=12345&redirect_uri=https://developer.microsoft.com/en-us/graph/")
}

# 理解ServicePricipal
Get-AzureADServicePrincipal 
Get-AzureADServicePrincipal -ObjectId 3319d71d-8dfc-42ff-8fa0-0aa64f553350 #Microsoft Graph这个服务组件
