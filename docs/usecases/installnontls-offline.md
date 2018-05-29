```powershell
import-module $PSScriptRoot\..\Thycotic.RabbitMq.Helper.PSCommands.dll

$path = "$PSScriptRoot"
install-Connector -rabbitMqUsername SITEUN -rabbitMqPw SITEPW -offlineErlangInstallerPath $path\Offline\o-erlang.exe -offlineRabbitMqInstallerPath $path\Offline\o-rabbitMq.exe -Verbose
```