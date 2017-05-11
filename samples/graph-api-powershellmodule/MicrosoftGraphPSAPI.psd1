@{
    RootModule ="MicrosoftGraphPSAPI.psm1"
    ModuleVersion="1.0"
    GUID="a10b0aa4-9a7a-4864-b027-629101135592"
    Author="陈希章"
    CompanyName="Microsoft"
    CopyRight=""
    Description=""
    PowerShellVersion="3.0"
    RequiredAssemblies = @('Microsoft.IdentityModel.Clients.ActiveDirectory.dll', 
               'Microsoft.IdentityModel.Clients.ActiveDirectory.WindowsForms.dll')
    FunctionsToExport="*"
    CmdletsToExport="*"
    VariablesToExport="*"
    AliasesToExport="*"
    ModuleList=@("MicrosoftGraphPSAPI.psm1")
    FileList = 'Microsoft.IdentityModel.Clients.ActiveDirectory.dll', 
               'Microsoft.IdentityModel.Clients.ActiveDirectory.WindowsForms.dll'
}