# Installation

## Overview
The RabbitMq Helper is a tool that streamlines the RabbitMq installation on Windows. See [Downloading and Installing RabbitMQ](https://www.rabbitmq.com/download.html) and [Installing on Windows](https://www.rabbitmq.com/install-windows.html) for vanilla instructions. 

While the helper has individual cmdlets that perform the different installation/un-installation steps, it also contains two workflow cmdlets called Install-Connector and Uninstall-Connector.

## Installation using ```Install-Connector``` workflow cmdlet
* License prompts and fine-print
* ```Get-ErlangInstaller``` - Download the installer for Erlang that it currently supported by Thycotic.
* ```Get-RabbitMqInstaller``` - Download the installer for RabbitMq that it currently supported by Thycotic.
* ```Uninstall-RabbitMq``` - Uninstall prior installation (if any) of RabbitMq and clean up
* ```Uninstall-Erlang``` - Uninstall prior installation (if any) of Erlang and clean up
* ```Set-ErlangHomeEnvironmentalVariable``` - Set Erlang and RabbitMq environmental variables
* ```Install-Erlang``` - Install Erlang
* ```New-RabbitMqConfigDirectory``` - Create a custom RabbitMq configuration directory
* ```Set-RabbitMqBaseEnvironmentalVariable``` - Set the RabbitMq BASE environmental variable to the created configuration directory
* With TLS
    * ```Convert-CaCerToPem``` - Convert CA certificate to Pem
    * ```Convert-PfxToPem``` - Convert Host PFX to Pem
    * ```New-RabbitMqTlsConfigFiles``` - Create RabbitMq TLS configuration file
    * ```Install-RabbitMq``` - Install RabbitMq
    * ```Copy-ErlangCookieFile``` - Copy the Erlang system cookie to the current user profile
    * ```Assert-RabbitMqIsRunning``` - Assert RabbitMq is running
    * ```Enable-RabbitMqManagement``` - Enable RabbitMq management UI
    * ```New-RabbitMqUser``` - Create a basic user 
    * ```Grant-RabbitMqUserPermission``` - Grant permissions to the created user
    * ```Assert-RabbitMqConnectivity``` - Assert the newly create user can connect to RabbitMq with TLS
    * ```Open-RabbitMqManagement``` - Open the RabbitMq management UI
* Without TLS
    * ```New-RabbitMqNonTlsConfigFiles``` - Create RabbitMq non-TLS configuration file
    * ```Install-RabbitMq``` - Install RabbitMq
    * ```Copy-ErlangCookieFile``` - Copy the Erlang system cookie to the current user profile
    * ```Assert-RabbitMqIsRunning``` - Assert RabbitMq is running
    * ```Enable-RabbitMqManagement``` - Enable RabbitMq management UI
    * ```New-RabbitMqUser``` - Create a basic user 
    * ```Grant-RabbitMqUserPermission``` - Grant permissions to the created user
    * ```Assert-RabbitMqConnectivity``` - Assert the newly create user can connect to RabbitMq without TLS
    * ```Open-RabbitMqManagement``` - Open the RabbitMq management UI

## Un-installation using ```Uninstall-Connector``` workflow cmdlet
* ```Uninstall-RabbitMq``` - Uninstall prior installation (if any) of RabbitMq and clean up
* ```Uninstall-Erlang``` - Uninstall prior installation (if any) of Erlang and clean up

## Use cases

- [Simple installation of RabbitMq without TLS from a computer connected to the Internet](installnontls.md)
- [Simple installation of RabbitMq without TLS from a computer NOT connected to the Internet](installnontls-offline.md)
- [Advanced installation of RabbitMq with TLS from a computer connected to the Internet](installtls.md)
- [Advanced installation of RabbitMq with TLS from a computer NOT connected to the Internet](installtls-offline.md)
- [Un-install RabbitMq and Erlang](uninstall.md)