[title]: # (Online RabbitMq Install w/o TLS)
[tags]: # (rabbitmq,installation)
[priority]: # (110)

# Online Simple Installation without TLS

```powershell
$cred = Get-Credential -Message "Enter the initial RabbitMq user username and password";
#if you don't want to be prompted you can hardcode your credential in the script
#$password = ConvertTo-SecureString “PlainTextPassword” -AsPlainText -Force
#$cred = New-Object System.Management.Automation.PSCredential (“CustomUserName”, $password)

Install-Connector `
    -Credential $cred `
    -UseThycoticMirror -Verbose
```

There are more switches for this commandlet, your run "get-help install-connector" when inside the helper for more information