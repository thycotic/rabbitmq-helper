# Preparing for installation on a computer NOT connected to the Internet

You an easily prepare an offline install. This is useful in cases where the host you are trying to install RabbitMq on does not outbound Internet access and therefore cannot download any installers directly.

- Install the helper on a host that *does* have access to the Internet
- Run the following example.
- Copy the downloaded installers to the target host
- Conduct the desired off-line installation on the target host

```powershell
#This can be any path you like
$path = "$env:programfiles\Thycotic Software Ltd\RabbitMq Helper\Examples";

Get-ErlangInstaller `
    -PrepareForOfflineInstall `
    -OfflineErlangInstallerPath "$path\Offline\o-erlang.exe" `
    -UseThycoticMirror `
    -Verbose;

Get-RabbitMqInstaller `
    -PrepareForOfflineInstall `
    -OfflineRabbitMqInstallerPath "$path\Offline\o-rabbitMq.exe" `
    -UseThycoticMirror `
    -Verbose;

#Use the -Force switch to force download even if the files are present

```

## Download locations

To simply get the download locations for the installers run the following example

```powershell
Get-DownloadLocations;
#or using the Thycotic CND mirror
Get-DownloadLocations -UseThycoticMirror;
```

- Copy the Offline folder the host which does not have internet and proceed with the desired installation.

## Related 

- [Simple installation of RabbitMq without TLS from a computer NOT connected to the Internet](installnontls-offline.md)
- [Advanced installation of RabbitMq with TLS from a computer NOT connected to the Internet](installtls-offline.md)