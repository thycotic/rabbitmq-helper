# Troubleshooting

## Cannot Delete epmd.exe after Uninstalling RabbitMQ and Erlang

This error happens when you uninstall RabbitMQ (and optionally Erlang) but cannot delete one or more programs or folders associated with RabbitMQ or Erlang. 

One common example would be "epmd.exe." This occurs because these files are still being held open by an active process and can present a problem if you uninstalled in order to do a clean reinstall. To fix this:

- Open command prompt as Administrator and run
```dos
tasklist
```
- Find the epmd.exe process (or whichever process cannot be deleted) and note the process ID

```dos
taskkill /pid {PROCESSID} /F
```

- Delete the file or folde

## Cannot remove Erlang cookie during installation

Example error: Failed to copy system cookie: Access to the path 'C:\Users\user1\.erlang.cookie' is denied.. Manual deletion might be necessary;Access to the path 'C:\Users\user1\.erlang.cookie' is denied.;. Would you like to Retry?

Work-around: Manually delete the cookie from the location the helper has no access to and retry.

## Management Plugin Does Not Load

The management plugin is the web page where the user logs in to manage RabbitMQ. This is usually located at [http://localhost:15672](http://localhost:15672)

When installing RabbitMQ using the helper:
- the management plugin should be enabled automatically
- the specified user should be automatically created and
- user should have appropriate permissions granted.

However, if any error occurs during the installation while using the Helper, the management plugin may not have been enabled. There are two ways to fix this:

1) Open the Helper and run
```powershell
Enable-RabbitMqManagement -Verbose
```

2) Open the RabbitMq Command Prompt and run

```dos
rabbitmq-plugins enable rabbitmq_management
```