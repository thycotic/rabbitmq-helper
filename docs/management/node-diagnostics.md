[title]: # (Node Diagnostics)
[tags]: # (rabbitmq,diagnostics)
[priority]: # (1)

# RabbitMQ Node Diagnostics

There a number of cmdlets in the Helper that allow you extract information about your cluster nodes and queues. You can use standard Powershell syntax to fit your needs. Here are some examples for a node where the Helper is installed. The -Verbose switch can be used for more details.

# User Credential Validation
```powershell
Assert-RabbitMqConnectivity

$usercred = Get-Credential -Message "Enter the RabbitMq user username and password to validate";
Assert-RabbitMqConnectivity -Credential $usercred

Assert-RabbitMqConnectivity -Hostname Nefarian -Credential $usercred

```

## Node and Queue Information
```powershell
#check to make sure RabbitMq is running
Assert-RabbitMqIsRunning

$admincred = Get-Credential -Message "Enter the administrative user RabbitMq user username and password";

#get the cluster name
Get-RabbitMqClusterName -AdminCredential $admincred

#get all cluster nodes
$nodes = Get-RabbitMqClusterNodes -AdminCredential $admincred

#select basic node information
$nodes | Select name,type,running

#request the current node to conduct a health-check 
Request-RabbitMqHealthCheck -AdminCredential $admincred

$queues = Get-RabbitMqQueues -AdminCredential $admincred

#select all queues which have a node as their master node
$queues | Where-Object {$_.node -eq 'rabbit@Nefarian'} | Select name | format-list
$queues | Where-Object {$_.node -eq 'rabbit@Nefarian'} | Measure

#select all queues which do not have a node as their master node
$queues | Where-Object {$_.node -ne 'rabbit@Nefarian'} | Select name,node | format-list
$queues | Where-Object {$_.node -ne 'rabbit@Nefarian'} | Measure

#select all queues which have a node as their salve node
$queues | Where-Object {$_.slave_nodes.Contains('rabbit@Nefarian')} | Select name,node | format-list

#select all queues and their respective effective policies
$queues | Select name,effective_policy_definition | format-list

```
