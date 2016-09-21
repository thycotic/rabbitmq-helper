import-module .\Release\Thycotic.RabbitMq.Helper.PSCommands.dll

$path = "M:\development\vso\DistributedEngine\Thycotic.RabbitMq.Helper\src\Examples"
install-Connector -verbose -hostname THYCOPAIR23.testparent.thycotic.com -useSsl -rabbitMqUsername SITEUN1 -rabbitMqPw SITEPW1 -cacertpath $path\sc.cer -pfxPath $path\sc.pfx -pfxPw password1 -agreeErlangLicense -agreeRabbitMqLicense
