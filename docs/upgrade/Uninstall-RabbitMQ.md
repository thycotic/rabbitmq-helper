# Uninstall RabbitMQ

## Purpose

* This guide is to walk through the process of upgrading RabbitMQ, the current process for this involves uninstalling completely, and then re-installing RabbitMQ using the approved Thycotic RabbitMQ Helper.

## Pre-Requisites

* RabbitMQ is currently installed in the environment and may be clustered or federated. Take note of clustering / federation configuration policies before the upgrade. To find these:

*	Navigate to the Local RabbitMQ node.
*	Open a web-browser, and browse to http://localhost:15672
*	Log-In to the RabbitMQ Management portal with administrative username and password. (Default is guest/guest)
*	Click the Policies Tab.
*	Take note of any configuration items that exist here.

## Uninstall

*	Navigate to the Control Panel > Programs and Features > RabbitMQ Helper > Remove / Uninstall
*	Install the newest version of the [RabbitMQ Helper](https://updates.thycotic.net/links.ashx?RabbitMqInstaller).
*	Once the new Helper has Installed, ensure you are logged into the system as a local user who is an explicit member of the Local Administrators Group. 
*	Navigate to the RabbitMQ Helper folder (Default is C:\Program Files\Thycotic Software Ltd\RabbitMq Helper)
*	Once there, right click the RabbitMQ Helper executable: Thycotic.RabbitMq.Helper.exe, and choose, run as Administrator. This will create the RabbitMQ Administrative Powershell Session that has the Powershell Module loaded into it already.
*	If your RabbitMQ instance is a member of a cluster, run the following command in the RabbitMQ Helper:
```powershell

Reset-RabbitMQNodeCommand -force

```

*	To uninstall the current version of RabbitMQ, run the following command in the RabbitMQ Helper:

```powershell

# Uninstall-Connector will remove the RabbitMQ Site connector, Erlang, and RabbitMQ.
Uninstall-Connector

```
    >   If you receive any errors in this step, run Uninstall-Connector again until no errors are returned.

##	Verify all pieces of Erlang and RabbitMQ are removed:

*	Browse to “C:\Program Files”, ensure that the folder erl9.3 (Note: the numbers next to the folder name may change, based on the current version of Erlang Installed) has been removed, if it has not, attempt to delete this folder.

*	If you are unable to delete any folder due to file locks, a reboot will be required, after the reboot has finished, the file locks should be removed, and you should be able to delete the folder.

*	Verify all .erlang.cookie files have been deleted, the 3 possible locations are:

    * User cookie if both HOMEDRIVE and HOMEPATH environment variables are set: %HOMEDRIVE%%HOMEPATH%\.erlang.cookie 	(Ususally C:\Windows\.erlang.cookie)

    * User cookie if both HOMEDRIVE and HOMEPATH environment variables are not set:  %USERPROFILE%\.erlang.cookie  (Usually C:\Users\CURRENTUSER\.erlang.cookie)

    * Service: For the RabbitMQ Windows service - %USERPROFILE%\.erlang.cookie (usually C:\WINDOWS\system32\config\systemprofile)  

*	In “C:\Program Files”, ensure that the folder “RabbitMQ Server” has been removed, if it has not, attempt to delete it.

*	Next, browse to “C:\Users\USER_THAT_INSTALLED_RABBITMQ\AppData\Roaming\RabbitMQ”

    >	Replace “USER_THAT_INSTALLED_RABBITMQ”, with the username of the user who originally installed RabbitMQ.

*	Ensure that the RabbitMQ folder located in the Roaming Profile has been deleted, if it has not, delete it. 

* If the RabbitMQ Service in Services.msc still exists, it needs to be removed. From and administrative command prompt run the following command:
   ```cmd 
   sc delete rabbitmq
   ```
##	Procedure Complete
* RabbitMQ is now fully uninstalled, and can be re-installed to upgrade to the latest version.
