# Management Plugin Does Not Load

The management plugin is the web page where the user logs in to manage RabbitMQ. This is usually located at [http://localhost:15672](http://localhost:15672). 

When installing RabbitMQ using the helper:
- the management plugin should be automatically enabled 
- the specified user should be automatically createdand 
- user should have appropriate permissions granted. 

However, if any error occurs during the installation while using the Helper, the management plugin may not have been enabled. There are two ways to fix this:
 
1) Open the Helper and run
```powershell
Enable-RabbitMqManagement
```

2) Open the RabbitMq Command Prompt and run

```dos
rabbitmq-plugins enable rabbitmq_management
```