[title]: # (Convert PFX to PEM)
[tags]: # (rabbitmq,tls)
[priority]: # (1)

# Convert a host PFX to pem file

RabbitMQ only supports PEM File format for certificate verification. More on certificates and TLS Support for RabbitMQ can be found at https://www.rabbitmq.com/ssl.html

The new PEM file created below will be placed into %PROGRAMFILES%\Thycotic Software Ltd\RabbitMq Site Connector\

## Example Localhost.pfx

The Examples folder contains a test localhost.pfx. This PFX is strictly for testing TLS on a single machine. **You have to import in the Personal/Certificates certificate store** since it is not a valid certificate issued by a CA and is not trusted. Any connections made to RabbitMQ when this certificate is used will otherwise fail.

```powershell
$path = "$env:programfiles\Thycotic Software Ltd\RabbitMq Helper\Examples";

$pfxCred = Get-Credential -UserName PfxUserName -Message "Enter the PFX password. Username is ignored";
#if you don't want to be prompted you can hardcode your credential in the script
#$password = ConvertTo-SecureString “PlainTextPassword” -AsPlainText -Force
#$pfxCred = New-Object System.Management.Automation.PSCredential (“Ignored”, $password)

Convert-PfxToPem `
    -PfxPath "$path\localhost.pfx" `
    -PfxCredential $pfxCred `
    -Verbose;
```

