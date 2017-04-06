import-module $PSScriptRoot\..\Thycotic.RabbitMq.Helper.PSCommands\bin\Release\Thycotic.RabbitMq.Helper.PSCommands.dll

install-Connector -rabbitMqUsername SITEUN -rabbitMqPw SITEPW -offlineErlangInstallerPath Offline\o-erlang.exe -offlineRabbitMqInstallerPath Offline\o-rabbitMq.exe