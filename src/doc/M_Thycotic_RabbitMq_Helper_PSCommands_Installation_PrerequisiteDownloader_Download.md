# PrerequisiteDownloader.Download Method 
 _**\[This is preliminary documentation and is subject to change.\]**_

Downloads the prerequisite.

**Namespace:**&nbsp;<a href="N_Thycotic_RabbitMq_Helper_PSCommands_Installation">Thycotic.RabbitMq.Helper.PSCommands.Installation</a><br />**Assembly:**&nbsp;Thycotic.RabbitMq.Helper.PSCommands (in Thycotic.RabbitMq.Helper.PSCommands.dll) Version: 7.0.0.0 (7.0.0.0)

## Syntax

**C#**<br />
``` C#
public void Download(
	CancellationToken token,
	string downloadUrl,
	string installerPath,
	string checksum,
	bool forceDownload = false,
	int maxRetries = 5,
	Action<string> debugHandler = null,
	Action<string> infoHandler = null,
	Action<string, Exception> warnHandler = null,
	Action<PrerequisiteDownloaderProgress> progressHandler = null
)
```


#### Parameters
&nbsp;<dl><dt>token</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/dd384802" target="_blank">System.Threading.CancellationToken</a><br />The token.</dd><dt>downloadUrl</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />The download URL.</dd><dt>installerPath</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />The installer path.</dd><dt>checksum</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />The installer checksum.</dd><dt>forceDownload (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/a28wyd50" target="_blank">System.Boolean</a><br />if set to `true` [force download].</dd><dt>maxRetries (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">System.Int32</a><br />The maximum retries.</dd><dt>debugHandler (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/018hxwa8" target="_blank">System.Action</a>(<a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">String</a>)<br />The debug handler.</dd><dt>infoHandler (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/018hxwa8" target="_blank">System.Action</a>(<a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">String</a>)<br />The information handler.</dd><dt>warnHandler (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/bb549311" target="_blank">System.Action</a>(<a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">String</a>, <a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a>)<br />The warn handler.</dd><dt>progressHandler (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/018hxwa8" target="_blank">System.Action</a>(<a href="T_Thycotic_RabbitMq_Helper_PSCommands_Installation_PrerequisiteDownloaderProgress">PrerequisiteDownloaderProgress</a>)<br />The progress handler.</dd></dl>

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="http://msdn2.microsoft.com/en-us/library/dzyy5k3x" target="_blank">FileNotFoundException</a></td><td>Failed to download</td></tr></table>

## See Also


#### Reference
<a href="T_Thycotic_RabbitMq_Helper_PSCommands_Installation_PrerequisiteDownloader">PrerequisiteDownloader Class</a><br /><a href="N_Thycotic_RabbitMq_Helper_PSCommands_Installation">Thycotic.RabbitMq.Helper.PSCommands.Installation Namespace</a><br />