[title]: # (RabbitMq Federation)
[tags]: # (rabbitmq,federation)
[priority]: # (10)

# Federation

The RabbitMQ Helper is a tool that streamlines the RabbitMQ federation process on Windows. See [Federation Plugin](https://www.rabbitmq.com/federation.html)

> The Helper does not assist with load-balancing. See [Load Balancing](../loadbalancing.md) for details.

## Federation Workflow

When RabbitMQ is installed on a virtual/physical machine, it is already in a cluster of one node. Federation enables clustered and non-clustered nodes to off-load work to each other. To federate to a remote node, we simply need to install RabbitMQ on a different virtual/physical machine and then establish an upstream connection to another node/cluster. 

The basic premise of federation is that the downstream server connects to the upstream server just like a consumer would. This means that same port clients used to connect to the node/cluster are the same clients that the port the downstream will use.

> To proceed, please be sure you have at least two RabbitMQ node already installed following the appropriate [installation process](../installation)

## Creating an Upstream Server

### Preliminary Steps
* Install RabbitMQ on N+1 virtual/physical machines
* Ensure that the downstream can resolve the upstream IP address
* Open firewall for cluster ports. 
    * Make sure the firewall rule is open for the network type (public/private/domain) on each virtual/physical machine where RabbitMQ nodes will be installed.
    * TCP ports: 5672 (default) but can be configured
* Enable federation support on the node/cluster that will be creating the upstream server.
* Create a basic user on the upstream server which will be used by the downstream server to connect.

> If you are establishing one-way federation, federation only needs to be enabled on the downstream server.

> If you plan on using the same username on both nodes. You will have to create it on both. Keep in mind, federated servers don't share policies or users. This is the opposite of clusters.

> The helper does not currently support TLS upstreams.



### Steps Using the Helper (On Downstream Only)

> The upstream server does not require any additional plug-ins or configuration steps.

* ```Enable-RabbitMqFederationAndManagement``` - Enables the federation and federation management UI 
* ```Set-RabbitMqFederationUpstream``` - Set the upstream server

```powershell
#on the node to add the upstreak

Enable-RabbitMqFederationAndManagement -Verbose

$cred = Get-Credential -Message "Enter the upstream user RabbitMq user username and password";
$admincred = Get-Credential -Message "Enter the administrative user RabbitMq user username and password";

Set-RabbitMqFederationUpstream -Hostname HOSTNAMEORFQDN -Name fed-test -Credential $cred -AdminCredential $admincred -FirewallConfigured -Verbose
```

## Removing the Upstream Server

Log into the RabbitMQ management console and remove the upstream server from **Admin > Federation Upstreams**

## Establishing a Federation Policy

### Simple Federation of non-HA Queues

*Coming soon*

### HA Queue Federation

> The options listed here are cluster policies that can be extended to leverage federation:

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
