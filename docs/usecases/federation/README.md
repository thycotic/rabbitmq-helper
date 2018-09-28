# Federation

The RabbitMq Helper is a tool that streamlines the RabbitMq federation process on Windows. See [Federation Plugin](https://www.rabbitmq.com/federation.html)

> The helper does not assist with load-balancing. See [Load balancing](../loadbalancing.md) for details.

## Federation workflow

When RabbitMq is installed on a virtual/physical machine, it is already in a cluster of one node. Federation enables clustered and non-clustered nodes to off-load work to each other. To federate to a remote node, we simply need to install RabbitMq on a different virtual/physical machine and then establish an upstream to another node/cluster. 

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

> If you plan on using the same username on both nodes. You will have to create it on both. Keep in mind, federated servers don't share policies or users. This is the opposite of clusters.

>> The helper does not currently support TLS upstreams.



### Steps using the helper (on downstream only)

> Upstream server does not need any additional plugs or configuration

* ```Enable-RabbitMqFederationAndManagement``` - Enables the federation and federation management UI 
* ```Set-RabbitMqFederationUpstream``` - Set the upstream

```powershell
#on the node to add the upstreak

Enable-RabbitMqFederationAndManagement -Verbose

$cred = Get-Credential -Message "Enter the upstream user RabbitMq user username and password";
$admincred = Get-Credential -Message "Enter the administrative user RabbitMq user username and password";

Set-RabbitMqFederationUpstream -Hostname HOSTNAMEORFQDN -Name fed-test -Credential $cred -AdminCredential $admincred -FirewallConfigured -Verbose
```

## Removing the upstream

Log into the RabbitMq management you and remove the upstream from Admin -> Federation Upstreams

## Establishing a federation policy

### Simple federation of non-HA queues

*Coming soon*

### HA queue federation

> The options listed here are cluster policies that can be extended to leverage federation

* ```Set-RabbitMqBalancedClusterPolicy``` - creates a balanced cluster policy that distributes queues evenly around the cluster nodes and is also being federated

```powershell
$admincred = Get-Credential -Message "Enter the administrative user RabbitMq user username and password";

Set-RabbitMqBalancedClusterPolicy -Name fed-test-all -Pattern "^ActiveNonSslRabbitMq\:.*" -AdminCredential $admincred -IncludeInFederation

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

> IMPORTANT: Federation will not initiate for queues that are not bound or have never been consumed, even if all policies are correct. In order words, If node A is the upstream of B and B has consumers, unless A was consumed at least once, the queues in B will be created but will be empty.
