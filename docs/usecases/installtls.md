# Advanced installation of RabbitMq with TLS from a computer connected to the Internet

## General TLS requirements

- The certificate used has match the hostname used by RabbitMq. Wildcard certificates are supported
- The certificate has to be an RSA with 2048 bit encryption or higher (CNG and ECC have not been tested)
- The certificate chain has to be trusted by both the RabbitMq node and anything connecting to the RabbitMq host
    - If using self-signed certificated, ensure that the certificates are properly installed in the certificate store.

## Prompt for initial username/password

```powershell
install-Connector -hostname localhost -useSsl -cacertpath $path\sc.cer -pfxPath $path\sc.pfx -pfxPw password1 -UseThycoticMirror -Verbose
```

## Specify the initial username/password

```powershell
install-Connector -hostname localhost -useSsl -rabbitMqUsername SITEUN1 -rabbitMqPw SITEPW1 -cacertpath $path\sc.cer -pfxPath $path\sc.pfx -pfxPw password1 -UseThycoticMirror -Verbose
```