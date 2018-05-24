#Please replace the localhost.cer and localhost.pfx files with your own certificate files
import-module $PSScriptRoot\..\Thycotic.RabbitMq.Helper.PSCommands.dll

$path = "$PSScriptRoot"
install-Connector -hostname localhost -useSsl -rabbitMqUsername SITEUN -rabbitMqPw SITEPW -cacertpath "$path\localhost.cer" -pfxPath "$path\localhost.pfx" -pfxPw password1 -offlineErlangInstallerPath $path\Offline\o-erlang.exe -offlineRabbitMqInstallerPath $path\Offline\o-rabbitMq.exe -Verbose