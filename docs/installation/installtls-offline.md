[title]: # (Offline RabbitMq Install w/ TLS)
[tags]: # (rabbitmq,installation,offline)
[priority]: # (140)

# Offline Advanced Installation with TLS

## Preparation

You have to have the [Erlang and RabbitMq installers pre-downloaded](prepare-offline.md) for this step. Otherwise, installation will fail.

## General TLS requirements

- The certificate used has to match the hostname used by RabbitMq. Wildcard certificates are supported.
- The certificate has to be an RSA with 2048 bit encryption or higher for the RabbitMq Helper to be able to convert it.
    - CNG and/or ECC certificates can be manually converted with OpenSSL, see [Convert a CNG or ECC certificate to PEM File](certificate/convert-cngecctopem.md)
- The certificate chain has to be trusted by both the RabbitMq node and anything connecting to the RabbitMq host.
    - If using self-signed certificated, ensure that the certificates are properly installed in the certificate store.

```powershell
$path = "$env:programfiles\Thycotic Software Ltd\RabbitMq Helper\Examples";

$cred = Get-Credential -Message "Enter the initial RabbitMq user username and password";
#if you don't want to be prompted you can hardcode your credential in the script
#$password = ConvertTo-SecureString "PlainTextPassword" -AsPlainText -Force
#$cred = New-Object System.Management.Automation.PSCredential ("CustomUserName", $password)

# FQDN which will be used by clients connecting to this RabbitMq host. *It has to match the subject name in the PFX*
$fqdn = "fullyqualifieddomainname.in.the.pfx";

$certpath = $path;

$pfxCred = Get-Credential -UserName PfxUserName -Message "Enter the PFX password. Username is ignored";
#(the password for the example localhost.pfx certificate is "password1")
#if you don't want to be prompted you can hardcode your credential in the script
#$password = ConvertTo-SecureString "PlainTextPassword" -AsPlainText -Force
#$pfxCred = New-Object System.Management.Automation.PSCredential ("Ignored", $password)

Install-Connector `
    -Hostname $fqdn `
    -Credential $cred `
    -UseTls `
    -CaCertPath "$certpath\localhostca.cer" `
    -PfxPath "$certpath\localhost.pfx" `
    -PfxCredential $pfxCred `
    -OfflineErlangInstallerPath "$path\Offline\o-erlang.exe" `
    -OfflineRabbitMqInstallerPath "$path\Offline\o-rabbitMq.exe" `
    -Verbose
```

There are more switches for this commandlet, your run "get-help install-connector" when inside the helper for more information
