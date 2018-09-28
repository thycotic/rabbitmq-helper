# Convert a CA certificate to PFX to pem file

Pem file will be placed in to %PROGRAMFILES%\Thycotic Software Ltd\RabbitMq Site Connector\

## Example Localhostca.cer

The Examples folder contains a test localhostca.cer. This CER is strictly for testing TLS on a single machine. **You have to import it in the machine Truster Root Certification Authorities** since it not a real CA certificate and is not trusted. Any connections made to RabbitMq when this certificate is used will otherwise fail.

```powershell
$path = "$env:programfiles\Thycotic Software Ltd\RabbitMq Helper\Examples";

#Use a real CA cert in production unless there are good reasons not to
Convert-CaCertToPem `
    -CaCertPath "$path\localhostca.cer" `
    -Verbose;
```
