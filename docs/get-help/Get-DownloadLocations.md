# Get-DownloadLocations

```powershell
NAME
    Get-DownloadLocations
    
SYNOPSIS
    Gets download locations for Erlang and RabbitMq (most helpful when needing to do offline installation)
    
    
SYNTAX
    Get-DownloadLocations [-UseThycoticMirror <SwitchParameter>] [<CommonParameters>]
    
    
DESCRIPTION
    
    

PARAMETERS
    -UseThycoticMirror <SwitchParameter>
        Gets or sets a value indicating whether to use the Thycotic Mirror even if the file exists.
        
    -Mirror <SwitchParameter>
        Gets or sets a value indicating whether to use the Thycotic Mirror even if the file exists.
        
        This is an alias of the UseThycoticMirror parameter.
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216). 
    
    ----------  EXAMPLE 1  ----------
    
    PS C:\>Get-DownloadLocations
    
    ----------  EXAMPLE 2  ----------
    
    PS C:\>Get-DownloadLocations -UseThycoticMirror
    
REMARKS
    To see the examples, type: "get-help Get-DownloadLocations -examples".
    For more information, type: "get-help Get-DownloadLocations -detailed".
    For technical information, type: "get-help Get-DownloadLocations -full".
    For online help, type: "get-help Get-DownloadLocations -online"
```

