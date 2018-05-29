# Preparing for installation on a computer NOT connected to the Internet

You an easily prepare an offline install, if the host you are trying to install RabbitMq on does not outbound Internet access and therefore cannot download any installers directly.

- Install the helper on a host that *does* have access to the Internet
- Run the following prepare-offline.ps1 example or the following snip-it

```powershell
#This can be any path you like
$path = "%PROGRAMFILES%\Thycotic Software Ltd\RabbitMq Helper\Examples\Offline"

Get-ErlangInstaller -PrepareForOfflineInstall -OfflineErlangInstallerPath "$path\Offline\o-erlang.exe" -UseThycoticMirror -Verbose
Get-RabbitMqInstaller -PrepareForOfflineInstall -OfflineRabbitMqInstallerPath "$path\Offline\o-rabbitMq.exe" -UseThycoticMirror -Verbose
```

- To simply get the download locations for the installers run the following snip-it
```powershell
Get-DownloadLocations
#or
Get-DownloadLocations -UseThycoticMirror
```

- Copy the Offline folder the host which does not have internet and proceed with the desired installation.

## Related 

- [Simple installation of RabbitMq without TLS from a computer NOT connected to the Internet.](installnontls-offline.md)
- [Advanced installation of RabbitMq with TLS from a computer NOT connected to the Internet.](installtls-offline.md)