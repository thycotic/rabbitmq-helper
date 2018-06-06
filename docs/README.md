# RabbitMq Helper
The RabbitMQ Helper is a tool whose purpose is to assist with installing [RabbitMq](https://www.rabbitmq.com)

RabbitMq is not a Thycotic product and we do not receive revenue for it. We built the RabbitMq Helper to help our customers more easily install RabbitMq (RabbitMq is an important component of Secret Server’s on-premises environment).

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

- Download and install the latest RabbitMq Helper
    - Stable:
        - [Thycotic Updates - Alias to the Thycotic CDN](https://updates.thycotic.net/links.ashx?RabbitMqInstaller)
        - [Directly from Thycotic CDN](https://thycocdn.azureedge.net/engineinstallerfiles-master/rabbitMqSiteConnector/grmqh.msi)
    - Pre-release
        - [Pre-release drop](https://thycodevstorage.blob.core.windows.net/engineinstallerfiles-qa/rabbitMqSiteConnector/grmqh.msi)

- Start the helper. This will prepare and run a PowerShell instance that is pre-configured to use the helper PowerShell module.
    - Use the Start Menu shortcut for "Thycotic RabbitMq Helper PowerShell Host"
    - Or navigate to the installation folder in "%PROGRAMFILES%\Thycotic Software Ltd\RabbitMq Helper"
- Run PowerShell command-lets directly or use any of the example scripts provided.

## Use cases

### Preparation

- [Preparing for installation on a computer NOT connected to the Internet](usecases/prepare-offline.md)

### Installation

- [Simple installation of RabbitMq without TLS from a computer connected to the Internet](usecases/installnontls.md)
- [Simple installation of RabbitMq without TLS from a computer NOT connected to the Internet](usecases/installnontls-offline.md)
- [Advanced installation of RabbitMq with TLS from a computer connected to the Internet](usecases/installtls.md)
- [Advanced installation of RabbitMq with TLS from a computer NOT connected to the Internet](usecases/installtls-offline.md)

### Clustering

*Coming soon*

### Federation

*Coming soon*

### Maintenance

- [Remove all queues on a RabbitMq node](usecases/remove-all-queues.md)

## Having an issue?
- [Review the common troubleshooting tips](troubleshooting.md)
- If you are still having difficulties, [submit an issue on Github](https://github.com/thycotic/rabbitmq-helper/issues)

## Feedback & comments
    - Email dkolev@compfreak.com?subject=rabbitmq-helper-feedback

