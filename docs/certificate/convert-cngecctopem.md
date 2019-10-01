[title]: # (Convert CNG or ECC to PEM)
[tags]: # (rabbitmq,tls,CNG,ECC,PEM,OpenSSL)
[priority]: # (10)

# Convert a CNG or ECC certificate to PEM files

RabbitMq now supports CNG and/or ECC certificates, however the RabbitMq Helper can not convert these types from PFX currently. OpenSSL can be used to do the conversion instead:

```
openssl pkcs12 -in localhost.pfx -nocerts -out cert.key -nodes
openssl pkcs12 -in localhost.pfx -clcerts -nokeys -out cert.pem
```

# How to use CNG or ECC certificates with the helper

1. Run the OpenSSL commands above to convert your CNG or ECC PFX certificate to a cert.key and cert.pem file.
2. Follow the [Convert a CA Certificate PFX to PEM File](convert-cacerttopem.md) instructions to generate your ca.pem file.
3. Follow the relevant instructions to install RabbitMq with TLS enabled, using the localhost certs the Examples folder.
    - [Advanced installation of RabbitMQ with TLS from a computer connected to the Internet](../installation/installtls.md)
    - [Advanced installation of RabbitMQ with TLS from a computer NOT connected to the Internet](../installation/installtls-offline.md)
4. Replace the example cert.key, cert.pem, and ca.pem in `C:\RabbitMq\` with your files.
5. Restart the RabbitMq service using the `Stop-RabbitMq` and `Start-RabbitMq` helper commands.
