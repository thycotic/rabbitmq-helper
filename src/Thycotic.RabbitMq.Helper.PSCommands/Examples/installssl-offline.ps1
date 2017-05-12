#Please replace the sc.cer and sc.pfx files with your own certificate files
import-module $PSScriptRoot\..\Thycotic.RabbitMq.Helper.PSCommands.dll

$path = "$PSScriptRoot"
install-Connector -hostname localhost -useSsl -rabbitMqUsername SITEUN -rabbitMqPw SITEPW -cacertpath "$path\sc.cer" -pfxPath "$path\sc.pfx" -pfxPw password1 -offlineErlangInstallerPath $path\Offline\o-erlang.exe -offlineRabbitMqInstallerPath $path\Offline\o-rabbitMq.exe -Verbose