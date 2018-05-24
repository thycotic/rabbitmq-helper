#Please replace the localhost.cer and localhost.pfx files with your own certificate files
import-module $PSScriptRoot\..\Thycotic.RabbitMq.Helper.PSCommands.dll

$path = "$PSScriptRoot"
install-Connector -hostname localhost -useSsl -rabbitMqUsername SITEUN1 -rabbitMqPw SITEPW1 -cacertpath $path\localhost.cer -pfxPath $path\localhost.pfx -pfxPw password1 -UseThycoticMirror -Verbose
