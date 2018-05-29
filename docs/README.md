# RabbitMq Helper
The Rabbit MQ Helper is a tool whose purpose is to assist with installing [Rabbit Mq](https://www.rabbitmq.com)

Rabbit Mq is not a Thycotic product and we do not receive revenue for it. We built the Rabbit Mq Helper to help our customers more easily install Rabbit Mq (Rabbit is an important component of Secret Server’s on-premises environment).

## What can the helper do for me?

*The RabbitMq Helper currently, and for the forseeable future, only works on Windows OS.*

- Online and Offline Install RabbitMq with or without TLS
- Convert certificates for use with RabbitMq
- Establish RabbitMq clusters and streamline cluster policies
- Establish RabbitMq federations and streamline federation policies
- Enable management UI
- Create basic users
- View/manage the RabbitMq log 

## Getting started

- Download and install the latest RabbitMq Helper from (LINK COMING AFTER migration)
- Start the helper. This will prepare and run a PowerShell instance that is pre-configured to use the helper PowerShell module.
- Run PowerShell command-lets directly or use any of the example scripts provided.

## Use cases

### Installation
- [Simple installation of RabbitMq without TLS from a computer connected to the Internet.](usecases/installnontls.md)
- [Simple installation of RabbitMq without TLS from a computer NOT connected to the Internet.](usecases/installnontls-offline.md)
- [Advanced installation of RabbitMq with TLS from a computer connected to the Internet.](usecases/installtls.md)
- [Advanced installation of RabbitMq with TLS from a computer NOT connected to the Internet.](usecases/installtls-offline.md)

### Maintenance
- [Remove all current queues](usecases/remove-all-queues.md)

*More use cases coming soon*