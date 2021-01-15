[title]: # (RabbitMq and RabbitMQ Helper)
[tags]: # (rabbitmq,helper,powershell,cmdlet)
[priority]: # (1)

# RabbitMQ and RabbitMQ Helper

RabbitMQ is an important component of Secret Server’s on-premises environment, providing a robust framework for queuing messages between Secret Server and its Distributed Engines. RabbitMQ is an enterprise-ready software package that provides reliability and clustering functionality superior to other applications. For detailed information about RabbitMQ go to https://www.rabbitmq.com/. RabbitMQ is not a Thycotic product and we receive do revenue from it. We provide direct support for versions of RabbitMQ/Erlang only when they are installed through the Thycotic Rabbit MQ Helper in Windows.

## What can the Helper do for Me?

*The RabbitMQ Helper currently, and for the forseeable future, only works on Windows OS.*

- Online and offline install of RabbitMQ with or without TLS
- Convert certificates for use with RabbitMQ
- Establish RabbitMQ clusters and streamline cluster policies
- Establish RabbitMQ federations and streamline federation policies
- Enable management UI
- Create basic users
- View/manage the RabbitMQ log 

## Installation
   - [Installation Overview](installation/index.md)

## Advanced

### Clustering
   - [Clustering Overview](clustering/index.md)

### Certificates 
> Install-Connector workflow normally converts certificates without needing to use the below

- [Convert a CA Certificate PFX to PEM File](certificate/convert-cacerttopem.md)
- [Convert a Host PFX to PEM File](certificate/convert-pfxtopem.md)
- [Convert a CNG or ECC certificate to PEM File](certificate/convert-cngecctopem.md)


### Federation
- [Federation Overview](federation/index.md)


## Troubleshooting and Maintenance
   - [RabbitMq Node Diagnostics](management/node-diagnostics.md)
   - [Remove All Queues on a RabbitMq Node](management/remove-all-queues.md)
   - [Review Common Troubleshooting Tips](troubleshooting.md)
   - [Get-Help Output for all cmdlets](get-help/index.md)
   - If you are still having difficulties, [submit an issue on Github](https://github.com/thycotic/rabbitmq-helper/issues)
