[title]: # (Offline RabbitMq Install w/o TLS)
[tags]: # (rabbitmq,installation,offline)
[priority]: # (120)

# Offline Simple Installation without TLS

## Preparation

You have to have the [Erlang and RabbitMq installers pre-downloaded](prepare-offline.md) for this step. Otherwise, installation will fail.

```powershell
$path = "$env:programfiles\Thycotic Software Ltd\RabbitMq Helper\Examples";

$cred = Get-Credential -Message "Enter the initial RabbitMq user username and password";
#if you don't want to be prompted you can hardcode your credential in the script
#$password = ConvertTo-SecureString “PlainTextPassword” -AsPlainText -Force
#$cred = New-Object System.Management.Automation.PSCredential (“CustomUserName”, $password)

Install-Connector `
    -Credential $cred `
    -OfflineErlangInstallerPath $path\Offline\o-erlang.exe `
    -OfflineRabbitMqInstallerPath $path\Offline\o-rabbitMq.exe `
    -Verbose;
```

There are more switches for this commandlet, your run "get-help install-connector" when inside the helper for more information