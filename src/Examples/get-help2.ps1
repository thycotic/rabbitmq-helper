$moduleName = "Thycotic.RabbitMq.Helper.PSCommands";
$modulePath = "$PSScriptRoot\..\Thycotic.RabbitMq.Helper.PSCommands\bin\Debug\$moduleName.dll"


Import-module $modulePath
#get-help get-help -full
#get-help Convert-CaCertToPem -full 
#Convert-CaCertToPem -CaCertPath "$PSScriptRoot\..\Examples\sc.cer" -Verbose
#get-help Convert-PfxToPem -full
#Convert-PfxToPem -PfxPath "$PSScriptRoot\..\Examples\sc.pfx" -PfxPassword "password1" -Verbose

#get-help Copy-RabbitMqExampleNonSslConfigFile -full
#Copy-RabbitMqExampleNonSslConfigFile -Verbose
#get-help Copy-RabbitMqExampleSslConfigFile -full
#get-help Get-DownloadLocations -full
#get-help Get-ErlangInstaller -full
#get-help Get-RabbitMqInstaller -full
#Get-RabbitMqInstaller -Verbose
#Get-RabbitMqInstaller -Verbose -Force
get-help Install-Connector -full
#Install-Connector
#get-help Install-Erlang -full
#get-help Install-RabbitMq -full
#get-help New-RabbitMqConfigDirectory -full
#get-help Set-ErlangHomeEnvironmentalVariable -full
#get-help Set-RabbitMqBaseEnvironmentalVariable -full
#get-help UnInstall-Connector -full
#get-help UnInstall-Erlang -full
#get-help UnInstall-RabbitMq -full
#
#
#get-help Assert-RabbitMqConnectivity -full
#
#get-help Remove-AllQueues -full
#get-help Get-RabbitMqLog -full
#get-help Get-RabbitMqSaslLog -full
#get-help Enable-RabbitMqManagementPlugin -full
#get-help New-BasicRabbitMqUser -full


Remove-Module $moduleName
