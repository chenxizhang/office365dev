$files = Get-ChildItem -Path . -Filter "*.md"
foreach ($item in $files) {
    pandoc.exe -o "C:\temp\$($item.Name).docx" $item.Name
}
