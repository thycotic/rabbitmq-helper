import-module $PSScriptRoot\..\Thycotic.RabbitMq.Helper.PSCommands\bin\Release\Thycotic.RabbitMq.Helper.PSCommands.dll

install-Connector -verbose -rabbitMqUsername SITEUN1 -rabbitMqPw SITEPW1 -agreeErlangLicense -agreeRabbitMqLicense -force  -usethycoticmirror
