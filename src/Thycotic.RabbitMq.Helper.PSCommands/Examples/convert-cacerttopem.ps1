#Please replace the localhost.cer and localhost.pfx files with your own certificate files
import-module $PSScriptRoot\..\Thycotic.RabbitMq.Helper.PSCommands.dll

#Pem file will be placed in to C:\Program Files\Thycotic Software Ltd\RabbitMq Site Connector\
$path = "$PSScriptRoot"
Convert-CaCertToPem -CaCertPath "$path\localhost.cer" -Verbose
