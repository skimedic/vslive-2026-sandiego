## Build The DbContext and the DbContextFactory
You are a C# developer that specializes in Entity Framework Core.

Make sure to read and ingest the general instructions file (.github/copilot-instructions.md) and the C# instructions (.github/instructions/csharp.instructions.md).
All updates in this lab take place in the AutoLot.Dal Project.
The AutoLot.Models project contains the entity classes and the AutoLot.Dal project contains the DbContext and the DbContextFactory.

# Create Global Usings
Rename the class "Class1.cs" to "GlobalUsings.cs" and add the following code to it:
```csharp
global using AutoLot.Models.Entities;
global using AutoLot.Models.Entities.Base;
global using AutoLot.Models.Entities.Configuration;
global using AutoLot.Models.ViewModels;
global using AutoLot.Models.ViewModels.Configuration;

global using Microsoft.Data.SqlClient;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using Microsoft.EntityFrameworkCore.Metadata;
global using Microsoft.EntityFrameworkCore.Migrations;
global using Microsoft.EntityFrameworkCore.Query;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.Extensions.DependencyInjection;

global using System.Data;
global using System.Linq.Expressions;
```

# Create Folders
Create the following folder in the project:
- EfStructures

# Create the DbContext Class

## Class Name: ApplicationDbContext
Location: EfStructures  
Derives from: DbContext(options)  
Constructor parameters: DbContextOptions<ApplicationDbContext> options
Properties:
    Create a public DbSet property for each of the entities in the AutoLot.Models.Entities and 
      AutoLot.Models.ViewModels.Configuration namespaces

Override the OnModelCreating method to apply configurations from the AutoLot.Models.Entities.Configuration and AutoLot.Models.ViewModels.Configuration namespaces.

# Create the DesignTimeDbContextFactory Class

## Class Name: ApplicationDbContextFactory
Location: EfStructures  
Derives from: IDesignTimeDbContextFactory<ApplicationDbContext>  
Overridden CreateDbContext method:
```csharp
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = @"server=(localdb)\mssqllocaldb;Database=AutoLot_Hol;Trusted_Connection=True;";
        optionsBuilder.UseSqlServer(connectionString);
        //This is for teaching purposes only, you should not use this in production code.
        Console.WriteLine(connectionString);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
```

# Update the Global Usings File
Add the following to the "GlobalUsings.cs" file:
```csharp
global using AutoLot.Dal.EfStructures;
```
