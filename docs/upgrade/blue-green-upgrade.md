# Blue-Green Upgrade Procedure

## Purpose

This procedure is used to upgrade your RabbitMQ Cluster one node at a time to allow for continuous up-time of RabbitMQ services during an upgrade. The procedure is completed by breaking each node out of the cluster, upgrading it, then rejoining it to the cluster.

## Procedure

### Point Secret Server Site Connector at "Primary" Node
* Navigate in the Secret Server Web Interface to Admin > Distributed Engine > Manage Site Connectors

    * Choose the site connector you’d like to upgrade, and change the URL to point to the “Primary” RabbitMQ Node
    	* Depending on your LB configuration (and how traffic is routed), you may be able to leave this setting to the LB url and rely on the loadbalancer not to route traffic to the node you are upgrading.
	* Press the "Validate Connectivity" button and ensure Secret Server can still connect to RabbitMQ

### Remove "Secondary" Node from the Cluster
* Next, RDP into the "Secondary" RabbitMQ Node, install the lastest version of the [RabbitMQ Helper](https://updates.thycotic.net/links.ashx?RabbitMqInstaller)

	*  Log into the RabbitMQ Management GUI at http://localhost:15672, and take note of any cluster policies listed under Admin > Policies
	*  Once the helper has installed, navigate to the install directory (Default is C:\ProgramFiles\Thycotic Software ltd\RabbitMQ Helper) and run the Helper application. It will open an administrative Powershell Session.
		* On the "Secondary" Node:  ```Reset-RabbitMQNodeCommand -Force``` -  This command will remove the current node from the cluster. 
```powershell

Reset-RabbitMQNodeCommand -Force

```

### Uninstall 
* Follow the [uninstall instructions](./uninstall-rabbitmq.md) from the RabbitMQ Helper page.  

### Re-Install
* Follow the [install instructions](../usecases/installation) from the RabbitMQ Helper page.


### Point Secret Server Site Connector at Newly installed Node
* RabbitMQ is installed on the new node, navigate to Secret Server, and point the site connector at your newly installed node. You should be able to validate connectivity if the user was installed correctly.

### Repeat Steps for "Primary"
* Once RabbitMQ is pointed to the upgraded node, repeat the uninstallation / reinstallation process for the “primary” RabbitMQ node.

### Cluster Newly installed Nodes
* Follow the [Clustering Instructions](../usecases/clustering).

* Install the policies as previously annotated. After both nodes are upgraded and clustered, shift the Site connector URL back to the Load Balancer URL.
