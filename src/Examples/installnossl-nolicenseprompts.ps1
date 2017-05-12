import-module $PSScriptRoot\Release\Thycotic.RabbitMq.Helper.PSCommands.dll

install-Connector -rabbitMqUsername SITEUN1 -rabbitMqPw SITEPW1 -agreeErlangLicense -agreeRabbitMqLicense -UseThycoticMirror -Verbose
