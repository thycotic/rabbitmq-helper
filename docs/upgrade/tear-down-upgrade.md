# Tear-down Upgrade - Reinstallation

A tear down upgrade is not an upgrade per se but simply a reinstallation using the latest versions of RabbitMq and Erlang.

For a true upgrade, review the [blue-green upgrade](blue-green-upgrade.md) option.


## Basic Deployment

A basic deployment is defined as a single, non-clustered implementation of RabbitMQ. 

To upgrade your deployment:

- Halt all services using RabbitMq as a message broker.
- If you have the PowerShell script you used to install RabbitMq initially, you can use to reinstall both RabbitMq and Erlang. If you do not, review the available [use cases](..\readme.md) and go from there

## Clustered Deployment

A clustered deployment is defined as multiple RabbitMQ deployments used for a single site. To upgrade this deployment - we recommend following the instructions in this article to break up your cluster, upgrade using the instructions above for a basic deployment on each of your RabbitMQ deployments, and then use the same article to create your clusters again.

If you have any issues after trying these instructions, please contact our Support Team through the Support Portal.