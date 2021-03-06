# Select-Exception

```powershell
NAME
    Select-Exception
    
SYNOPSIS
    Selects the exception as well its inner exceptions. Optionally selects the stack trace
    
    
SYNTAX
    Select-Exception [-Exception] <Exception> [-IncludeStackTrace <SwitchParameter>] [<CommonParameters>]
    
    
DESCRIPTION
    The Select-Exception cmdlet enumerates the specified exception is generates a list of key-value pairs which could be selected further.
    

PARAMETERS
    -Exception <Exception>
        Gets or sets the exception.
        
    -PfxPw <Exception>
        Gets or sets the exception.
        
        This is an alias of the Exception parameter.
        
    -IncludeStackTrace <SwitchParameter>
        Gets or sets the include stack trace.
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216). 
    
    ----------  EXAMPLE 1  ----------
    
    PS C:\>try { Assert-RabbitMqConnectivity -UserName test15 -Password test15 -Hostname localhost } catch { Select-Exception $_.Exception}
    
REMARKS
    To see the examples, type: "get-help Select-Exception -examples".
    For more information, type: "get-help Select-Exception -detailed".
    For technical information, type: "get-help Select-Exception -full".
    For online help, type: "get-help Select-Exception -online"
```

