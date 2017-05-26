[xml]$xml = Get-Content .\packages.xml
$nodes = $xml.packages

foreach($item in $nodes.package){
	#Uninstall-Package $item.id
	Install-Package $item.id -Version $item.version
}