# Convert-PfxToPem

```powershell
NAME
    Convert-PfxToPem
    
SYNOPSIS
    Converts a PFX cert to a pem/key combination.
    
    
SYNTAX
    Convert-PfxToPem [-PfxPath] <string> -PfxCredential <PSCredential> [<CommonParameters>]
    
    
DESCRIPTION
    The Convert-PfxToPem cmdlet converts a PFX cert to a pem/key combination.
    
    The pem file will be located in the Thycotic RabbitMq Site Connector folder.
    

PARAMETERS
    -PfxPath <string>
        Gets or sets the PFX path.
        
    -PfxCredential <PSCredential>
        Gets or set the credential for the PFX. Username part is ignored.
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216). 
    
    ----------  EXAMPLE 1  ----------
    
    PS C:\>Convert-PfxToPem -PfxPath "$PSScriptRoot\..\Examples\sc.pfx" -PfxPassword "password1" -Verbose
    
REMARKS
    To see the examples, type: "get-help Convert-PfxToPem -examples".
    For more information, type: "get-help Convert-PfxToPem -detailed".
    For technical information, type: "get-help Convert-PfxToPem -full".
    For online help, type: "get-help Convert-PfxToPem -online"
```

