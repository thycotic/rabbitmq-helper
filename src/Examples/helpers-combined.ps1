$moduleName = "Thycotic.RabbitMq.Helper.PSCommands";
$modulePath = "$PSScriptRoot\Release\$moduleName.dll"

Import-module $modulePath

####### Complete commands

#get-help Install-Connector -full
#Install-Connector

#get-help UnInstall-Connector -full
#UnInstall-Connector -Verbose

####### Partial commands

#get-help Convert-CaCertToPem -full 
#Convert-CaCertToPem -CaCertPath "$PSScriptRoot\..\Examples\sc.cer" -Verbose

#get-help Convert-PfxToPem -full
#Convert-PfxToPem -PfxPath "$PSScriptRoot\..\Examples\sc.pfx" -PfxPassword "password1" -Verbose

#get-help Copy-RabbitMqExampleNonSslConfigFile -full
#Copy-RabbitMqExampleNonSslConfigFile -Verbose

#get-help Copy-RabbitMqExampleSslConfigFile -full
#Copy-RabbitMqExampleSslConfigFile -Verbose

#get-help Get-DownloadLocations -full
#Get-DownloadLocations
#Get-DownloadLocations -UseThycoticMirror

#get-help Get-ErlangInstaller -full
#Get-ErlangInstaller -Verbose
#Get-ErlangInstaller -Verbose -UseThycoticMirror -Force
#Get-ErlangInstaller -Verbose -Force

#get-help Get-RabbitMqInstaller -full
#Get-RabbitMqInstaller -Verbose
#Get-RabbitMqInstaller -Verbose -UseThycoticMirror -Force
#Get-RabbitMqInstaller -Verbose -Force

#get-help Set-ErlangHomeEnvironmentalVariable -full
#Set-ErlangHomeEnvironmentalVariable -Verbose

#get-help Set-RabbitMqBaseEnvironmentalVariable -full
#Set-RabbitMqBaseEnvironmentalVariable -Verbose

#get-help New-RabbitMqConfigDirectory -full
#New-RabbitMqConfigDirectory -Verbose

#get-help Install-Erlang -full
#Install-Erlang -Verbose

#get-help Install-RabbitMq -full
#Install-RabbitMq -Verbose

#get-help Enable-RabbitMqManagementPlugin -full
#Enable-RabbitMqManagementPlugin -Verbose
#Enable-RabbitMqManagementPlugin -OpenConsoleAfterInstall -Verbose

#get-help New-BasicRabbitMqUser -full
#New-BasicRabbitMqUser -UserName SITEUN -Password SITEPW -Verbose

#get-help Assert-RabbitMqConnectivity -full
#Assert-RabbitMqConnectivity -HostName localhost -username SITEUN -password SITEPW -Verbose
# only if installed with SSL
#Assert-RabbitMqConnectivity -UseSsl -hostname localhost -username SITEUN -password SITEPW -Verbose

#get-help Remove-AllQueues -full
#Remove-AllQueues -Verbose #not implemented

#get-help Get-RabbitMqLog -full
#Get-RabbitMqLog
#Get-RabbitMqLog -Count 1000

#get-help Get-RabbitMqSaslLog -full
#Get-RabbitMqSaslLog
#Get-RabbitMqSaslLog -Count 1000

#get-help UnInstall-RabbitMq -full
#UnInstall-RabbitMq -Verbose

#get-help UnInstall-Erlang -full
#UnInstall-Erlang -Verbose

Remove-Module $moduleName
