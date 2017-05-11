Add-Type -TypeDefinition @"
    public enum Office365Version {
            Global,
            Gallatin
    }
"@


<#
.Synopsis
    获取访问令牌
.Description
    获取访问令牌
.EXAMPLE
    Get-GraphAuthToken
.NOTES
    XXX
.LINK
    http://graph.microsoft.io
#>
function Get-GraphAuthToken {
    param(
        [string]$ClientId,
        [string]$RedirectUri,
        [pscredential]$Credential,
        [Office365Version]$Version
    )

    switch ($Version) {
        "Global" { 
            Write-Output "Global";
            break
         }
        "Gallatin" {
            Write-Output "Gallatin";
            break;
        }
        Default {
            Write-Output "Global";
            break
        }
    }
}


