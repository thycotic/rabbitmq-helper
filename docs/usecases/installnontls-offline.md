# Simple installation of RabbitMq without TLS from a computer NOT connected to the Internet

*You have to have the Erlang and RabbitMq installers pre-downloaded for this step*

## Prompt for initial username/password

```powershell
install-Connector -offlineErlangInstallerPath $path\Offline\o-erlang.exe -offlineRabbitMqInstallerPath $path\Offline\o-rabbitMq.exe -Verbose
```

## Specify the initial username/password

```powershell
install-Connector -rabbitMqUsername SITEUN -rabbitMqPw SITEPW -offlineErlangInstallerPath $path\Offline\o-erlang.exe -offlineRabbitMqInstallerPath $path\Offline\o-rabbitMq.exe -Verbose
```