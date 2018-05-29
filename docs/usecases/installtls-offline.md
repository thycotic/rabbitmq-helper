# Advanced installation of RabbitMq with TLS from a computer NOT connected to the Internet

*You have to have the Erlang and RabbitMq installers pre-downloaded for this step*

## General TLS requirements

- The certificate used has match the hostname used by RabbitMq. Wildcard certificates are supported
- The certificate has to be an RSA with 2048 bit encryption or higher (CNG and ECC have not been tested)
- The certificate chain has to be trusted by both the RabbitMq node and anything connecting to the RabbitMq host
    - If using self-signed certificated, ensure that the certificates are properly installed in the certificate store.

## Prompt for initial username/password

```powershell
install-Connector -hostname myfqdn -useSsl -cacertpath "$path\mycaert.cer" -pfxPath "$path\myfqdncert.pfx" -pfxPw password1 -offlineErlangInstallerPath $path\Offline\o-erlang.exe -offlineRabbitMqInstallerPath $path\Offline\o-rabbitMq.exe -Verbose
```

## Specify the initial username/password

```powershell
install-Connector -hostname myfqdn -useSsl -rabbitMqUsername SITEUN -rabbitMqPw SITEPW -cacertpath "$path\mycaert.cer" -pfxPath "$path\myfqdncert.pfx" -pfxPw password1 -offlineErlangInstallerPath $path\Offline\o-erlang.exe -offlineRabbitMqInstallerPath $path\Offline\o-rabbitMq.exe -Verbose
```