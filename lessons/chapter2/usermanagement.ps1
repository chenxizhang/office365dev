# 这个脚本演示如何通过PowerShell管理Office 365用户
# 作者：陈希章 《Office 365 开发入门指南》第二版  于 2019-2-9

# 准备工作，参考 https://docs.microsoft.com/zh-cn/office365/enterprise/powershell/connect-to-office-365-powershell
# 安装一个登陆助手 https://go.microsoft.com/fwlink/p/?LinkId=286152

# 通过单独打开一个Powershell窗口，并用管理员方式打开，运行下面的命令安装模块，请注意这两个模块不一
# Install-Module -Name Msonline

# 第一步，连接到国际版的Office 365
Connect-MsolService 

# 连接到国内版本的Office 365
Connect-MsolService -AzureEnvironment AzureChinaCloud

# 获取所有的用户列表
Get-MsolUser

# 获取还没有授权的用户列表
Get-MsolUser -UnlicensedUsersOnly

# 创建新用户
New-MsolUser

# 修改用户
Set-MsolUser

# 删除用户
Remove-MsolUser

# 创建组
New-MsolGroup

# 修改组
Set-MsolGroup

# 删除组
Remove-MsolGroup

# 添加组成员
Add-MsolGroupMember

# 获取组成员
Get-MsolGroupMember

# 移除组成员
Remove-MsolGroupMember
