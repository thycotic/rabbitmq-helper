#Please replace the sc.cer and sc.pfx files with your own certificate files
import-module ..\Thycotic.RabbitMq.Helper.PSCommands\bin\Release\Thycotic.RabbitMq.Helper.PSCommands.dll

install-Connector -verbose -rabbitMqUsername SITEUN1 -rabbitMqPw SITEPW1 -agreeErlangLicense -agreeRabbitMqLicense
