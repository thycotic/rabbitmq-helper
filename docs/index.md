[title]: # (RabbitMq Helper)
[tags]: # (rabbitmq,helper,powershell,cmdlet)
[priority]: # (1)

# RabbitMQ Helper

The RabbitMQ Helper is a tool whose purpose is to assist with installing [RabbitMQ](https://www.rabbitmq.com)

RabbitMQ is not a Thycotic product and we do not receive revenue for it. We built the RabbitMQ Helper to help our customers more easily install RabbitMQ (RabbitMQ is an important component of Secret Server’s on-premises environment).

## What can the Helper do for Me?

*The RabbitMQ Helper currently, and for the forseeable future, only works on Windows OS.*

- Online and offline install of RabbitMQ with or without TLS
- Convert certificates for use with RabbitMQ
- Establish RabbitMQ clusters and streamline cluster policies
- Establish RabbitMQ federations and streamline federation policies
- Enable management UI
- Create basic users
- View/manage the RabbitMQ log 

## Prerequisites

- Download and install the latest RabbitMQ Helper
    - Stable:
        - [Thycotic Updates - Alias to the Thycotic CDN](https://updates.thycotic.net/links.ashx?RabbitMqInstaller)
        - [Directly from Thycotic CDN](https://thycocdn.azureedge.net/engineinstallerfiles-master/rabbitMqSiteConnector/grmqh.msi)
    - Pre-release
        - [Pre-release drop](https://thycodevstorage.blob.core.windows.net/engineinstallerfiles-qa/rabbitMqSiteConnector/grmqh.msi)

- Start the Helper. This will prepare and run a PowerShell instance that is pre-configured to use the Helper PowerShell module.
    - Use the Start Menu shortcut for "Thycotic RabbitMQ Helper PowerShell Host"
    - Or navigate to the installation folder in "%PROGRAMFILES%\Thycotic Software Ltd\RabbitMq Helper"
- Run PowerShell commandlets directly or use any of the example scripts provided.

 
   ##### For Offline Installs:
   - [Preparing for Installation on a Computer NOT Connected to the Internet](installation/prepare-offline.md)


## Installation
   - [Installation Overview](installation/index.md)


## Advanced

### Clustering
   - [Clustering Overview](clustering/index.md)

### Certificates 
> Install-Connector workflow normally converts certificates without needing to use the below

- [Convert a CA Certificate to PFX to PEM File](certificate/convert-cacerttopem.md)
- [Convert a Host PFX to PEM File](certificate/convert-pfxtopem.md)


### Federation
- [Federation Overview](federation/index.md)


## Troubleshooting and Maintenance
   - [RabbitMq Node Diagnostics](management/node-diagnostics.md)
   - [Remove All Queues on a RabbitMq Node](management/remove-all-queues.md)
   - [Review Common Troubleshooting Tips](troubleshooting.md)
   - [Get-Help Output for all cmdlets](get-help/index.md)
   - If you are still having difficulties, [submit an issue on Github](https://github.com/thycotic/rabbitmq-helper/issues)
