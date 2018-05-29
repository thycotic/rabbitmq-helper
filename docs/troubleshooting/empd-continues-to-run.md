# Cannot Delete epmd.exe after Uninstalling RabbitMQ and Erlang

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