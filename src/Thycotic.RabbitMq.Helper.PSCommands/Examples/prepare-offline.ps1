import-module $PSScriptRoot\..\Thycotic.RabbitMq.Helper.PSCommands.dll


$path = "$PSScriptRoot"
Get-ErlangInstaller -PrepareForOfflineInstall -OfflineErlangInstallerPath "$path\Offline\o-erlang.exe" -UseThycoticMirror -Verbose
Get-RabbitMqInstaller -PrepareForOfflineInstall -OfflineRabbitMqInstallerPath "$path\Offline\o-rabbitMq.exe" -UseThycoticMirror -Verbose