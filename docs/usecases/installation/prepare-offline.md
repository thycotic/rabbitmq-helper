# Preparing for Installation on a Computer NOT Connected to the Internet

You can easily prepare an offline install for RabbitMQ. This is useful in cases where the host you are trying to install RabbitMQ on does not have outbound Internet access and therefore cannot download any installers directly.

To perform an offline install, follow these steps:

- Install the RabbitMQ Helper on a host that *does* have access to the Internet
- Run the example script listed below on the machine where the Helper is downloaded 
- Copy the downloaded installers (Helper & RabbitMQ) to the target (offline) host machine
- Conduct the desired off-line installation on the target host by running the installers

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

## Download Locations

To simply get the download locations for the installers, run the following example:

```powershell
Get-DownloadLocations;
#or using the Thycotic CND mirror
Get-DownloadLocations -UseThycoticMirror;
```

- Copy the offline folder to the target host which does not have internet and proceed with the desired installation.

## Related 

- [Simple installation of RabbitMq without TLS from a computer NOT connected to the Internet](installnontls-offline.md)
- [Advanced installation of RabbitMq with TLS from a computer NOT connected to the Internet](installtls-offline.md)
