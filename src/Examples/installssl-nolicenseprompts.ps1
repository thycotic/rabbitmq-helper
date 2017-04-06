#Please replace the sc.cer and sc.pfx files with your own certificate files
import-module $PSScriptRoot\..\Thycotic.RabbitMq.Helper.PSCommands\bin\Release\Thycotic.RabbitMq.Helper.PSCommands.dll

$path = "M:\development\vso\DistributedEngine\Thycotic.RabbitMq.Helper\src\Examples"
install-Connector -verbose -hostname localhost -useSsl -rabbitMqUsername SITEUN1 -rabbitMqPw SITEPW1 -cacertpath $path\sc.cer -pfxPath $path\sc.pfx -pfxPw password1 -agreeErlangLicense -agreeRabbitMqLicense
