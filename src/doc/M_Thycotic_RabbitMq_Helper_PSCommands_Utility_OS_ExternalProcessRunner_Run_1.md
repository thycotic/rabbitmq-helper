# ExternalProcessRunner.Run Method (String, String, String)
 _**\[This is preliminary documentation and is subject to change.\]**_

Runs the specified executable path.

**Namespace:**&nbsp;<a href="N_Thycotic_RabbitMq_Helper_PSCommands_Utility_OS">Thycotic.RabbitMq.Helper.PSCommands.Utility.OS</a><br />**Assembly:**&nbsp;Thycotic.RabbitMq.Helper.PSCommands (in Thycotic.RabbitMq.Helper.PSCommands.dll) Version: 7.0.0.0 (7.0.0.0)

## Syntax

**C#**<br />
``` C#
public void Run(
	string executablePath,
	string workingPath,
	string parameters = null
)
```


#### Parameters
&nbsp;<dl><dt>executablePath</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />The executable path.</dd><dt>workingPath</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />The working path.</dd><dt>parameters (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />The parameters.</dd></dl>

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="http://msdn2.microsoft.com/en-us/library/ww58ded5" target="_blank">ApplicationException</a></td><td>Process failed or Process appears to have failed</td></tr><tr><td><a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a></td><td /></tr></table>

## See Also


#### Reference
<a href="T_Thycotic_RabbitMq_Helper_PSCommands_Utility_OS_ExternalProcessRunner">ExternalProcessRunner Class</a><br /><a href="Overload_Thycotic_RabbitMq_Helper_PSCommands_Utility_OS_ExternalProcessRunner_Run">Run Overload</a><br /><a href="N_Thycotic_RabbitMq_Helper_PSCommands_Utility_OS">Thycotic.RabbitMq.Helper.PSCommands.Utility.OS Namespace</a><br />