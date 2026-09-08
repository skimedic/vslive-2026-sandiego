## Logging and other Shared Services
You are a C# developer that specializes in ASP.NET Core.

Make sure to read and ingest the general instructions file (.github/copilot-instructions.md) and the C# instructions (.github/instructions/csharp.instructions.md).
This entire lab takes place in the AutoLot.Services Project.

# Create Global Usings
Rename the class "Class1.cs" to "GlobalUsings.cs" and add the following code to it:
```csharp
global using AutoLot.Dal.Repos;
global using AutoLot.Dal.Repos.Base;
global using AutoLot.Dal.Repos.Interfaces;
global using AutoLot.Dal.Repos.Interfaces.Base;

global using AutoLot.Models.Exceptions;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;

global using Serilog;
global using Serilog.Context;
global using Serilog.Core.Enrichers;
global using Serilog.Events;
global using Serilog.Sinks.MSSqlServer;

global using System.Data;
global using System.Diagnostics;
global using System.Runtime.CompilerServices;
```

# Logging  

## Create Folders
Create the following folder and subfolders in the project:
- Logging
    - Configuration
    - Interfaces
    - Settings

## Create Classes and Interface
Create the following class:  

### Class Name: AppLoggerSettings   
Location: Logging/Settings  
Contained Classes:
- GeneralSettings
  - Properties:
    - RestrictedToMinimumLevel (string, required, AllowEmptyStrings = false)
- FileSettings
  - Properties:
    - Drive (string, required, AllowEmptyStrings = false)
    - FilePath (string, required, AllowEmptyStrings = false)
    - FileName (string, required, AllowEmptyStrings = false)
    - FullLogPathAndFileName (readonly string, value: $"{Drive}{Path.VolumeSeparatorChar}{Path.DirectorySeparatorChar}{FilePath}{Path.DirectorySeparatorChar}{FileName}")
- SqlServerSettings
  - Properties:
    - TableName (string, required, AllowEmptyStrings = false)
    - Schema (string, required, AllowEmptyStrings = false)
    - ConnectionStringName (string, required, AllowEmptyStrings = false)
Properties:  
- General (GeneralSettings - defined above, required)
- File (FileSettings - defined above, required)
- MSSqlServer (SqlServerSettings - defined above, required)

### Interface Name: IAppLogger  
Location: Logging/Interfaces  
Notes: In addition to any parameters indicated below, all methods take the following parameters:
- string message
- [CallerMemberName] string memberName = ""
- [CallerFilePath] string filePath = ""
- [CallerLineNumber] int lineNumber = 0

Methods:  
- LogAppError (void)  
  - Parameters:
    - Exception ex
- LogAppError (void)  
- LogAppCritical (void)  
  - Parameters:
    - Exception ex
- LogAppCritical (void)  
- LogAppDebug (void)  
- LogAppTrace (void)  
- LogAppInformation (void)  
- LogAppWarning (void)  

## Update the GlobalUsings.cs file
Add the following global usings to the GlobalUsings.cs file:
```csharp
global using AutoLot.Services.Logging;
global using AutoLot.Services.Logging.Interfaces;
global using AutoLot.Services.Logging.Settings;
```

## Class Name: AppLogger
Location: Logging  
Implements: IAppLogger   
Constructor Parameters:  
- ILogger<AppLogger> logger  

Methods:  
- LogWithException (void, internal)  
  - Parameters:
    - string memberName
    - string filePath
    - int lineNumber
    - string message
    - Exception ex
    - Action<Exception, string, object[]> logAction
  - Implementation:
```csharp
    var disposables =
      new List<IDisposable>
      {
        LogContext.PushProperty("MemberName", memberName),
        LogContext.PushProperty("FilePath", filePath),
        LogContext.PushProperty("LineNumber", lineNumber)
      };
    try
    {
      logAction(ex, message, Array.Empty<object>());
    }
    finally
    {
      foreach (var d in disposables)
      {
        d.Dispose();
      }
    }
```
- LogWithoutException (void, internal)  
  - Parameters:
    - string memberName
    - string filePath
    - int lineNumber
    - string message
    - Action<string, object[]> logAction
  - Implementation:
```csharp
    var disposables =
      new List<IDisposable>
      {
        LogContext.PushProperty("MemberName", memberName),
        LogContext.PushProperty("FilePath", filePath),
        LogContext.PushProperty("LineNumber", lineNumber)
      };
    try
    {
      logAction(message, Array.Empty<object>());
    }
    finally
    {
      foreach (var d in disposables)
      {
        d.Dispose();
      }
    }
```

The remainder of the interface functions will call either LogWithException or LogWithoutException with the appropriate parameters and logAction.

# String Extensions
Create new folder in the project named Utilities, and in the folder create a new static class named StringExtensions.cs. Add the following code to the class:
```csharp
public static class StringExtensions
{
  extension (string value)
  {
    public string RemoveControllerSuffix()
      => value != null && value.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
        ? value[..^10]
        : value;
    public string RemoveAsyncSuffix()
      => value != null && value.EndsWith("Async", StringComparison.Ordinal) 
         ? value[..^5] 
         : value;
  }
}
```
## Update the GlobalUsings.cs file
Add the following global usings to the GlobalUsings.cs file:
```csharp
global using AutoLot.Services.Utilities;
```

# Simple Service  
Create new folder in the project named Simple and a subfolder named Interfaces.   

## Interface Name: ISimpleService  
Location: Simple\Interfaces  
Method:  
- SayHello (string)  
  - Return Type: string  
  - Parameters: none  

## Class Name: SimpleServiceOne  
Location: Simple  
Implements: ISimpleService  
Method returns "Hello from SimpleServiceOne!"  

## Class Name: SimpleServiceTwo
Location: Simple
Implements: ISimpleService
Method returns "Hello from SimpleServiceTwo!"  

## Update the GlobalUsings.cs file
Add the following global usings to the GlobalUsings.cs file:
```csharp
global using AutoLot.Services.Simple;
global using AutoLot.Services.Simple.Interfaces;
```

# Dealer Info ViewModel  
Create new folder in the project named ViewModels.   

## Class Name: DealerInfo
Location: ViewModels  
Properties:  
- DealerName (string)
- City (string)
- State (string)

## Update the GlobalUsings.cs file
Add the following global usings to the GlobalUsings.cs file:
```csharp
global using AutoLot.Services.ViewModels;
```
