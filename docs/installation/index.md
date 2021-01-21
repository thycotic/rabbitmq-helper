[title]: # (Installation)
[tags]: # (rabbitmq,installation)
[priority]: # (100)

# Installing RabbitMQ with RabbitMQ Helper

## RabbitMQ and RabbitMQ Helper
RabbitMQ is an important component of Secret Server’s on-premises environment, providing a robust framework for queuing messages between Secret Server and its Distributed Engines. RabbitMQ is an enterprise-ready software package that provides reliability and clustering functionality superior to other applications. RabbitMQ is not a Thycotic product and we receive no revenue from it. For download links and detailed information about RabbitMQ go to https://www.rabbitmq.com/. 

We built the Thycotic RabbitMQ Helper application to streamline our customers’ RabbitMQ installations. Thycotic only supports RabbitMQ/Erlang installations performed using RabbitMQ Helper on the Windows OS. 

The Helper provides multiple commandlets that perform the installation/un-installation steps, along with two workflow commandlets named ```Install-Connector``` and ```Uninstall-Connector```.

## Install RabbitMQ and set up a Site Connector
1. Download RabbitMQ from the RabbitMQ [website](https://www.rabbitmq.com/). 
1. Check the RabbitMQ installation [prerequisites and set up a Site Connector](https://docs.thycotic.com/ss/10.9.0/secret-server-setup/installation/installing-rabbitmq/index.md) in Secret Server


## Install RabbitMQ Helper
> **Note**: Before installing RabbitMQ Helper, you must set up inbound firewall rules on the machine that is hosting the connector.
1. Download Rabbit MQ Helper at one of the links below:

   - [Directly from Thycotic CDN](https://thycocdn.azureedge.net/engineinstallerfiles-master/rabbitMqSiteConnector/grmqh.msi) 
   - [From Thycotic CDN alias](https://updates.thycotic.net/links.ashx?RabbitMqInstaller) 
   - [Pre-release](https://thycodevstorage.blob.core.windows.net/engineinstallerfiles-qa/rabbitMqSiteConnector/grmqh.msi) 

2.	Choose the most appropriate installation scenario from the four below, but **do not run the associated script yet**:

     - [Online Simple Installation without TLS](installnontls.md)
     - [Online Advanced Installation with TLS](installnontls-offline.md)
     - [Offline Simple Installation without TLS](installtls.md)
     - [Offline Advanced Installation with TLS](installtls-offline.md)

2. Launch the Helper (Thycotic.RabbitMq.Helper.exe) from the installation folder %PROGRAMFILES%\Thycotic Software Ltd\RabbitMq Helper. 
      Thycotic.RabbitMq.Helper.exe prepares and runs a Windows PowerShell instance that is pre-configured to use the RabbitMQ Helper PowerShell module.

4. Run PowerShell cmdlets that apply to your installation needs, or use the most appropriate script from the four choices listed above. 
 > Note: RabbitMQ Helper provides the built-in cmdlet, Install-Connector, which is a prerequisite for installing any of the sample installation scripts. **You MUST install RabbitMQ Helper before running any of the scripts**. If you choose to use one of the Offline installation scripts, you must first install the script on [this page](installation/prepare-offline.md).

After installation, the Helper opens a browser to the RabbitMQ management console, which you can close for now.

## Validate the RabbitMQ Helper Installation

1.	Return to Secret Server and navigate to the site connector you created previously.
6.	Click the site connector link. 
7.	On the Site Connector Details page, click the **Validate Connectivity** button.

    If see **Validation Succeeded**, everything is set up correctly.

    If you see **Validation Failed**, do the following: 
    1. Ensure that the RabbitMQ Windows service is running. 
    2. Check the logs found under **C:\Program Files\Thycotic Software Ltd\RabbitMq Site Connector\log**.
    3. Check the SS system log for a full error report.


## How to Uninstall Rabbit

> This command will remove both RabbitMq and Erlang from the current system. Before running this, if you’re having issues with Rabbit, it’s recommended to look at the troubleshooting section first.
- [Uninstall RabbitMQ and Erlang](uninstall.md)

## Overview of ```Install-Connector``` Workflow cmdlet 

> The overview below is provided for informational purposes in order to explain the process in the Install-Connector cmdlet. Wherever possible, use the 'Install-Connector" commandlet to install Rabbit and not the individual commands below. See example installation sections above
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


