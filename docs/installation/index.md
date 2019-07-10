[title]: # (Installation)
[tags]: # (rabbitmq,installation)
[priority]: # (100)

# Installation

## Overview
The RabbitMQ Helper is a tool that streamlines the RabbitMQ installation on Windows. See [Downloading and Installing RabbitMQ](https://www.rabbitmq.com/download.html) and [Installing on Windows](https://www.rabbitmq.com/install-windows.html) for vanilla instructions. 

While the Helper has individual cmdlets that perform the different installation/un-installation steps, it also contains two workflow cmdlets called ```Install-Connector``` and ```Uninstall-Connector```.

## Installation Examples with Arguments
***The below links show the four main scenarios when installing RabbitMQ using the Rabbit Helper and how to deploy Rabbit in each scenario. Pick the link which applies to you for example commands***
- [Simple installation of RabbitMQ without TLS from a computer connected to the Internet](installnontls.md)
- [Simple installation of RabbitMQ without TLS from a computer NOT connected to the Internet](installnontls-offline.md)
- [Advanced installation of RabbitMQ with TLS from a computer connected to the Internet](installtls.md)
- [Advanced installation of RabbitMQ with TLS from a computer NOT connected to the Internet](installtls-offline.md)

## How to Uninstall Rabbit
***This command will remove both RabbitMq and Erlang from the current system. Before running this, if you’re having issues with Rabbit, it’s recommended to look at the troubleshooting section first.***
- [Uninstall RabbitMQ and Erlang](uninstall.md)


## Overview of ```Install-Connector``` Workflow cmdlet
***The overview below is provided for informational purposes in order to explain the process in the Install-Connector cmdlet. Wherever possible, use the 'Install-Connector" commandlet to install Rabbit and not the individual commands below. See example installation sections above***
* License prompts and fine-print:
* ```Get-ErlangInstaller``` - Download the installer for Erlang that it currently supported by Thycotic.
* ```Get-RabbitMqInstaller``` - Download the installer for RabbitMQ that it currently supported by Thycotic.
* ```Uninstall-RabbitMq``` - Uninstall prior installation (if any) of RabbitMQ and clean up
* ```Uninstall-Erlang``` - Uninstall prior installation (if any) of Erlang and clean up
* ```Set-ErlangHomeEnvironmentalVariable``` - Set Erlang and RabbitMQ environmental variables
* ```Install-Erlang``` - Install Erlang
* ```New-RabbitMqConfigDirectory``` - Create a custom RabbitMQ configuration directory
* ```Set-RabbitMqBaseEnvironmentalVariable``` - Set the RabbitMQ BASE environmental variable to the created configuration directory
* With TLS:
    * ```Convert-CaCerToPem``` - Convert CA certificate to PEM file format
    * ```Convert-PfxToPem``` - Convert Host PFX to PEM file format
    * ```New-RabbitMqTlsConfigFiles``` - Create RabbitMQ TLS configuration file
    * ```Install-RabbitMq``` - Install RabbitMQ
    * ```Copy-ErlangCookieFile``` - Copy the Erlang system cookie to the current user profile
    * ```Assert-RabbitMqIsRunning``` - Assert RabbitMQ is running
    * ```Enable-RabbitMqManagement``` - Enable RabbitMQ management UI
    * ```New-RabbitMqUser``` - Create a basic user 
    * ```Grant-RabbitMqUserPermission``` - Grant permissions to the created user
    * ```Assert-RabbitMqConnectivity``` - Assert the newly create user can connect to RabbitMQ with TLS
    * ```Open-RabbitMqManagement``` - Open the RabbitMQ management UI
* Without TLS:
    * ```New-RabbitMqNonTlsConfigFiles``` - Create RabbitMQ non-TLS configuration file
    * ```Install-RabbitMq``` - Install RabbitMQ
    * ```Copy-ErlangCookieFile``` - Copy the Erlang system cookie to the current user profile
    * ```Assert-RabbitMqIsRunning``` - Assert RabbitMQ is running
    * ```Enable-RabbitMqManagement``` - Enable RabbitMQ management UI
    * ```New-RabbitMqUser``` - Create a basic user 
    * ```Grant-RabbitMqUserPermission``` - Grant permissions to the created user
    * ```Assert-RabbitMqConnectivity``` - Assert the newly create user can connect to RabbitMQ without TLS
    * ```Open-RabbitMqManagement``` - Open the RabbitMQ management UI

## Uninstall RabbitMQ Using the ```Uninstall-Connector``` Workflow cmdlet
* ```Uninstall-RabbitMq``` - Uninstall prior installation (if any) of RabbitMQ and clean up
* ```Uninstall-Erlang``` - Uninstall prior installation (if any) of Erlang and clean up


