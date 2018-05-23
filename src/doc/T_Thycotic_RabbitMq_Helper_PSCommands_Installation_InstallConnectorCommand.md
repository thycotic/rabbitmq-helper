# InstallConnectorCommand Class
 _**\[This is preliminary documentation and is subject to change.\]**_

Installs the site connector


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/ms582793" target="_blank">InternalCommand</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Thycotic.RabbitMq.Helper.PSCommands.Installation.InstallConnectorCommand<br />
**Namespace:**&nbsp;<a href="N_Thycotic_RabbitMq_Helper_PSCommands_Installation">Thycotic.RabbitMq.Helper.PSCommands.Installation</a><br />**Assembly:**&nbsp;Thycotic.RabbitMq.Helper.PSCommands (in Thycotic.RabbitMq.Helper.PSCommands.dll) Version: 7.0.0.0 (7.0.0.0)

## Syntax

**C#**<br />
``` C#
public class InstallConnectorCommand : PSCmdlet
```

The InstallConnectorCommand type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand__ctor">InstallConnectorCommand</a></td><td>
Initializes a new instance of the InstallConnectorCommand class</td></tr></table>&nbsp;
<a href="#installconnectorcommand-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_AgreeErlangLicense">AgreeErlangLicense</a></td><td>
Gets or sets the agree Erlang license. If omitted, the user will not be prompted to agree to the license.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_AgreeRabbitMqLicense">AgreeRabbitMqLicense</a></td><td>
Gets or sets the agree rabbit mq license. If omitted, the user will not be prompted to agree to the license.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_CaCertPath">CaCertPath</a></td><td>
Gets or sets the CA certificate path. This certificate is use to establish the trust chain to the CA.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dd128179" target="_blank">CommandOrigin</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582793" target="_blank">InternalCommand</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms581056" target="_blank">CommandRuntime</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dd128144" target="_blank">CurrentPSTransaction</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dd128219" target="_blank">Events</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_ForceDownload">ForceDownload</a></td><td>
Gets or sets a value indicating whether force download (even they already exist) the pre-requisites. This value has no effect when using an offline installer.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms581309" target="_blank">Host</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_Hostname">Hostname</a></td><td>
Gets or sets the hostname or FQDN of the server which will host the RabbitMq node.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms581310" target="_blank">InvokeCommand</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms581311" target="_blank">InvokeProvider</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/hh485055" target="_blank">JobManager</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dd128220" target="_blank">JobRepository</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms581312" target="_blank">MyInvocation</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_OfflineErlangInstallerPath">OfflineErlangInstallerPath</a></td><td>
Gets or sets the offline Erlang installer path. If omitted, the installer will be downloaded.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_OfflineRabbitMqInstallerPath">OfflineRabbitMqInstallerPath</a></td><td>
Gets or sets the offline RabbitMq installer path to use. If omitted, the installer will be downloaded.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/hh485057" target="_blank">PagingParameters</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms581313" target="_blank">ParameterSetName</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_Password">Password</a></td><td>
Gets or sets the RabbitMq password of the initial user.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_PfxPassword">PfxPassword</a></td><td>
Gets or sets the PFX password.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_PfxPath">PfxPath</a></td><td>
Gets or sets the PFX path. This could be a self-signed or a certificate from a public CA. If self-signed, the certificate should be installed on all client/engine machines. It does NOT to be installed on the RabbitMq node.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms581314" target="_blank">SessionState</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms581057" target="_blank">Stopping</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_UserName">UserName</a></td><td>
Gets or sets the name of the RabbitMq user name of the initial user.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_UseSsl">UseSsl</a></td><td>
Gets or sets whether to use SSL or not.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_UseThycoticMirror">UseThycoticMirror</a></td><td>
Gets or sets a value indicating whether to use the Thycotic Mirror even if the file exists.</td></tr></table>&nbsp;
<a href="#installconnectorcommand-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568245" target="_blank">BeginProcessing</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568785" target="_blank">CurrentProviderLocation</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568246" target="_blank">EndProcessing</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/4k87zsw7" target="_blank">Finalize</a></td><td>
Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as a hash function for a particular type.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568786" target="_blank">GetResolvedProviderPathFromPSPath</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568247" target="_blank">GetResourceString</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568787" target="_blank">GetUnresolvedProviderPathFromPSPath</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568788" target="_blank">GetVariableValue(String)</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568789" target="_blank">GetVariableValue(String, Object)</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms551396" target="_blank">PSCmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568248" target="_blank">Invoke</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/57ctke0a" target="_blank">MemberwiseClone</a></td><td>
Creates a shallow copy of the current <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_Thycotic_RabbitMq_Helper_PSCommands_Installation_InstallConnectorCommand_ProcessRecord">ProcessRecord</a></td><td>
Processes the record.
 (Overrides <a href="http://msdn2.microsoft.com/en-us/library/ms568254" target="_blank">Cmdlet.ProcessRecord()</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568259" target="_blank">ShouldContinue(String, String)</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568262" target="_blank">ShouldContinue(String, String, Boolean, Boolean)</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568267" target="_blank">ShouldProcess(String)</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568271" target="_blank">ShouldProcess(String, String)</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568273" target="_blank">ShouldProcess(String, String, String)</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568276" target="_blank">ShouldProcess(String, String, String, ShouldProcessReason)</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568281" target="_blank">StopProcessing</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568283" target="_blank">ThrowTerminatingError</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dd182395" target="_blank">TransactionAvailable</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568366" target="_blank">WriteCommandDetail</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568368" target="_blank">WriteDebug</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568369" target="_blank">WriteError</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568371" target="_blank">WriteObject(Object)</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568370" target="_blank">WriteObject(Object, Boolean)</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568372" target="_blank">WriteProgress</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568373" target="_blank">WriteVerbose</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/ms568374" target="_blank">WriteWarning</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/ms582518" target="_blank">Cmdlet</a>.)</td></tr></table>&nbsp;
<a href="#installconnectorcommand-class">Back to Top</a>

## Extension Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_Thycotic_RabbitMq_Helper_PSCommands_Utility_CmdletExtensions_AsChildOf">AsChildOf</a></td><td>
Returns the current command as a child of the specified command. Use this when establishing chains of commandlets.
 (Defined by <a href="T_Thycotic_RabbitMq_Helper_PSCommands_Utility_CmdletExtensions">CmdletExtensions</a>.)</td></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_Thycotic_RabbitMq_Helper_PSCommands_Utility_CmdletExtensions_InvokeImmediate">InvokeImmediate</a></td><td>
Invokes the command immediately.
 (Defined by <a href="T_Thycotic_RabbitMq_Helper_PSCommands_Utility_CmdletExtensions">CmdletExtensions</a>.)</td></tr></table>&nbsp;
<a href="#installconnectorcommand-class">Back to Top</a>

## Examples

The most basic use case to install RabbitMq is to have a single node without using encryption.

This is generally useful during development or during POC stages.

To do so, you could use the following:

PS C:\>

```
Install-Connector -rabbitMqUsername SITEUN -rabbitMqPw SITEPW -agreeErlangLicense -agreeRabbitMqLicense
```


## Examples

You can avoid being prompted to agree to Erlang or RabbitMq licenses (during automated deployment) use

To do so, you could use the following:

PS C:\>

```
Install-Connector -rabbitMqUsername SITEUN -rabbitMqPw SITEPW -agreeErlangLicense -agreeRabbitMqLicense
```


## Examples

The leverage secure communication between RabbitMq and its clients, you should use encryption.

To do so, you could use the following:

PS C:\>

```
Install-Connector -verbose -hostname RABBITHOST1.FQDN -useSsl -rabbitMqUsername SITEUN -rabbitMqPw SITEPW -cacertpath $path\sc.cer -pfxPath $path\sc.pfx -pfxPw SOMEPASSWORD
```


## See Also


#### Reference
<a href="N_Thycotic_RabbitMq_Helper_PSCommands_Installation">Thycotic.RabbitMq.Helper.PSCommands.Installation Namespace</a><br />