# Federation

The RabbitMq Helper is a tool that streamlines the RabbitMq federation process on Windows. See [Federation Plugin](https://www.rabbitmq.com/federation.html)

> The helper does not assist with load-balancing. See [Load balancing](../loadbalancing.md) for details.

## Federation workflow

When RabbitMq is installed on a virtual/physical machine, it is already in a cluster of one node. Federation enables clustered and non-clustered not off-load work to each other. To federate to a remote node, we simply need to install RabbitMq on a different virtual/physical machine and then establish an upstream to another node/cluster. 

The basic premise of federation is that the downstream server connects to the upstream just like a consumer would. This means that same port clients use to connect to the node/cluster is the same the port the downstream will use.

> To proceed, please be sure you have at least two RabbitMq node already installed following the appropriate [installation](../installation/README.md)

## Creating an upstream

### Preliminary steps
* Install RabbitMq on N+1 virtual/physical machines
* Ensure that the downstream can resolve the upstream IP address
* Open firewall for cluster ports. 
    * Make sure the firewall rule is open for the network type (public/private/domain) on each virtual/physical machine where RabbitMq nodes will be installed.
    * TCP ports: 5672 (default) but can be configured
* Enable federation support on the node/cluster that will be creating the upstream.
* Create a basic user on the upstream to be used by the downstream to connect.

> If you are establishing one-way federation, federation needs to only be enabled on the downstream.

> The helper does not currently support TLS upstreams.

### Steps using the helper (downstream only)
* ```Enable-RabbitMqFederationAndManagement``` - Enables the federation and federation management UI 
* ```Join-RabbitMqCluster``` - Join the other node in a cluster

```powershell
#on the node to join

#obviously use your own custom character cookie and not this value!
Set-ErlangCookieFileCommand -CookieContent MYCUSTOMSECURECOOKIE


$cred = Get-Credential -Message "Enter the upstream user RabbitMq user username and password";
$admincred = Get-Credential -Message "Enter the administrative user RabbitMq user username and password";


#using the CookieSet and FirewallConfigured will prevent the helper for prompting. Only use if you have actually already set the cluster cookie and you have configured your firewall
New-RabbitMqFederationUpstream -Hostname WIN-U6PS6TNGL8J -Name fed-test -Credential $cred -AdminCredential $admincred -FirewallConfigured -Verbose
```

## Removing the upstream

Log into the RabbitMq management you and remove the upstream from Admin -> Federation Upstreams

## Establishing a federation policy

### Simple federation of non-HA queues

TODO

### HA queue federation

* ```Set-RabbitMqBalancedClusterPolicy``` - creates a balanced cluster policy that distributes queues evenly around the cluster nodes and is also being federated

```powershell
$admincred = Get-Credential -Message "Enter the administrative user RabbitMq user username and password";

Set-RabbitMqBalancedClusterPolicy -Name fed-test-all -Pattern "^ActiveNonSslRabbitMq:" -AdminCredential $admincred -IncludeInFederation

# you can create a policy with a custom sync batch size. The default is 400 for Set-RabbitMqBalancedClusterPolicy because Thycotic products have a worst case scenario size for messages to be at 256KB. When a sync message is generated 256*400 = 100MB. Larger sync message can cause fragementation if there is latency or network connection drops between cluster node. Alter as needed
Set-RabbitMqBalancedClusterPolicy -Name fed-test-all -Pattern "^ActiveNonSslRabbitMq:" -AdminCredential $admincred -SyncBatchSize 100 -IncludeInFederation

# you can create a policy with a custom replica count batch size. The default is 2 for Set-RabbitMqBalancedClusterPolicy because anything higher puts strain on the cluster. 
# 2 replicas means 1 master and 1 mirror. Alter as needed
Set-RabbitMqBalancedClusterPolicy -Name fed-test-all -Pattern "^ActiveNonSslRabbitMq:" -AdminCredential $admincred -QueueReplicaCount 3 -IncludeInFederation

# you can create a policy with automatic sync mode. The default for Set-RabbitMqBalancedClusterPolicy is manual to avoid forcing a queue to automatically synchronize when a new mirror joins.
Set-RabbitMqBalancedClusterPolicy -Name fed-test-all -Pattern "^ActiveNonSslRabbitMq:" -AdminCredential $admincred -AutomaticSyncMode -IncludeInFederation

# you can create a policy with a combination of policy definitions
Set-RabbitMqBalancedClusterPolicy -Name fed-test-all -Pattern "^ActiveNonSslRabbitMq:" -AdminCredential $admincred -SyncBatchSize 100 -QueueReplicaCount 3 -AutomaticSyncMode -IncludeInFederation

```


### Alternate steps without the helper
```cmd
REM this policy is not ideal and is not balanced. Please use the helper if possible
rabbitmqctl set_policy cluster-test-all "^cluster\-test:" "{""ha-mode"":""all""}"
```
