## Add Custom Exceptions
You are a C# developer that specializes in Entity Framework Core.

Make sure to read and ingest the general instructions file (.github/copilot-instructions.md) and the C# instructions (.github/instructions/csharp.instructions.md).
All updates in this lab take place in the AutoLot.Dal and AutoLot.Models Projects.

# Create the Custom Exceptions
The custom exception is added to the AutoLot.Models project. Create a new folder in the AutoLot.Models project called "Exceptions"

## Create the foundational custom exception class

## Class Name: CustomException
Location: AutoLot.Models/Exceptions  
Derives from: Exception  
Constructor parameters: 
    - empty, 
    - string message
    - string message, Exception innerException
Properties: <none>

### Update the Global Usings Files
Update the GlobalUsings.cs file in the AutoLot.Models and AutoLot.Dal projects to include the following using statement:
```csharp
global using AutoLot.Models.Exceptions;
```

## Create the DAL specific custom exception class
There are three DAL specific custom exceptions. Create a new folder in the AutoLot.Dal project called "Exceptions" and add three new exception classes. 

## Class Name: CustomConcurrencyException
Location: AutoLot.Dal/Exceptions  
Derives from: CustomException  
Constructor parameters: 
    - empty, 
    - string message
    - string message, DbUpdateConcurrencyException innerException
Properties: <none>

## Class Name: CustomDbUpdateException
Location: AutoLot.Dal/Exceptions  
Derives from: CustomException  
Constructor parameters: 
    - empty, 
    - string message
    - string message, DbUpdateException innerException
Properties: <none>

## Class Name: CustomRetryLimitExceededException
Location: AutoLot.Dal/Exceptions  
Derives from: CustomException  
Constructor parameters: 
    - empty, 
    - string message
    - string message, RetryLimitExceededException innerException
Properties: <none>

### Update the Global Usings Files
Update the GlobalUsings.cs file in the AutoLot.Dal project to include the following using statement:
```csharp
global using AutoLot.Dal.Exceptions;
```