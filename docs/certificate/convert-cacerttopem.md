[title]: # (Convert CA cert to PEM)
[tags]: # (rabbitmq,tls,CA,PFX,PEM)
[priority]: # (1)

# Convert a CA Certificate to PFX to PEM File
RabbitMQ only supports PEM File format for certificate verification. More on certificates and TLS Support for RabbitMQ can be found at https://www.rabbitmq.com/ssl.html

The new PEM file created below will be placed into %PROGRAMFILES%\Thycotic Software Ltd\RabbitMq Site Connector\

## Example Localhostca.cer

The Examples folder contains a test localhostca.cer. This CER is strictly for testing TLS on a single machine. **You have to import it in the machine Trusted Root Certification Authorities** since it not a real CA certificate and is not trusted. Any connections made to RabbitMQ when this certificate is used will otherwise fail.

```powershell
$path = "$env:programfiles\Thycotic Software Ltd\RabbitMq Helper\Examples";

#Use a real CA cert in production unless there are good reasons not to
Convert-CaCertToPem `
    -CaCertPath "$path\localhostca.cer" `
    -Verbose;
```
