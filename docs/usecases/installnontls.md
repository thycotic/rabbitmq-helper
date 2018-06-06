# Simple installation of RabbitMq without TLS from a computer connected to the Internet

```powershell
$cred = Get-Credential -Message "Enter the initial RabbitMq user username and password";
#if you don't want to be prompted you can hardcode your credential in the script
#$password = ConvertTo-SecureString “PlainTextPassword” -AsPlainText -Force
#$cred = New-Object System.Management.Automation.PSCredential (“CustomUserName”, $password)

install-Connector `
    -Credential $cred `
    -UseThycoticMirror -Verbose
```

There are more switches for this commandlet, your run "get-help install-connector" when inside the helper for more information