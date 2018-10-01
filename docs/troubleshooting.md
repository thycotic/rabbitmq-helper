# Troubleshooting

## *Cannot Delete epmd.exe after Uninstalling RabbitMQ and Erlang*

This error happens when you uninstall RabbitMQ (and optionally Erlang) but cannot delete one or more programs or folders associated with RabbitMQ or Erlang. 

One common example would be "epmd.exe." This occurs because these files are still being held open by an active process and can present a problem if you uninstalled in order to perform a clean reinstall. To fix this,

- Open command prompt as Administrator and run:
```dos
tasklist
```
- Find the epmd.exe process (or whichever process cannot be deleted) and note the process ID:

```dos
taskkill /pid {PROCESSID} /F
```

- Delete the file or folder

## Cannot remove Erlang cookie during installation

Example error:
```
Failed to copy system cookie: Access to the path 'C:\Users\user1\\.erlang.cookie' is denied.. Manual deletion might be necessary;Access to the path 'C:\Users\user1\\.erlang.cookie' is denied.;. Would you like to Retry?
```

You can manually delete the cookie from the location the Helper has no access to and retry.

## Management Plugin Does Not Load 

The management plugin is the web page where the user logs in to manage RabbitMQ. This is usually located at [http://localhost:15672](http://localhost:15672)

When installing RabbitMQ using the helper:
- The management plugin should be enabled automatically
- The specified user should be automatically created and
- The user should have appropriate permissions granted

However, if any error occurs during the installation while using the Helper, the management plugin may not have been enabled. There are two ways to fix this:

1) Open the Helper and run:
```powershell
Enable-RabbitMqManagement -Verbose
```

2) Open the RabbitMq Command Prompt and run:

```dos
rabbitmq-plugins enable rabbitmq_management
```

## Getting Exceptions but Not Seeing Any Details

When running a Helper cmdlet you may run into an exception but you do not see any details or inner exceptions.

You can either wrap the cmdlet call and select the exception to get a list of key-value pairs of exception messages:

```powershell
try { Stop-RabbitMq } catch { Select-Exception $_.Exception}
```

Or you can select topmost/combined message:

```powershell
try { Stop-RabbitMq } catch { (Select-Exception $_.Exception)[0].Value}
```

- Need the PowerShell Command Help Content?

```powershell

#markdown links
Get-Command -Module Thycotic.RabbitMq.Helper.PSCommands  | Sort | % { Write-Host "Get-Help $_ -Full | Out-File $_.txt" };

#detailed help file generation script
Get-Command -Module Thycotic.RabbitMq.Helper.PSCommands  | Sort | % { Write-Host "Get-Help $_ -Detailed | Out-File $_.txt" };

#full help file generation script
Get-Command -Module Thycotic.RabbitMq.Helper.PSCommands  | Sort | % { Write-Host "Get-Help $_ -Full | Out-File $_.txt" };

```
