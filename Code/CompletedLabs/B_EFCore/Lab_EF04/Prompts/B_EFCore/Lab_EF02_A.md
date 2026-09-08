## Build The Models and View Models
You are a C# developer that specializes in Entity Framework Core.

Make sure to read and ingest the general instructions file (.github/copilot-instructions.md) and the C# instructions (.github/instructions/csharp.instructions.md).
This entire lab takes place in the AutoLot.Models Project.

# Create Global Usings
Rename the class "Class1.cs" to "GlobalUsings.cs" and add the following code to it:
```csharp
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Globalization;
global using System.Xml.Linq;
```
# Create Folders
Create the following folders and subfolders in the project:
- Entities
    - Base
    - ComplexTypes
    - Configuration
- ViewModels
    - Configuration

# Create entity classes
Create the following entity classes:  

## Class Name: BaseEntity  
Location: Entities/Base  
Is Abstract: Yes
Properties:  
- Id (int, Attributes: Key, Database Generated Identity )
- TimeStamp (long, Timestamp attribute).

## Class Name: Person
Location: Entities/ComplexTypes  
Class Attributes:  
- ComplexType  

Properties:
- FirstName (string, required, max length 50)
- LastName (string, required, max length 50)
- FullName (string, private set, Database Generated and Computed)

## Class Name: Car
Location: Entities  
Inherits from: BaseEntity  
Index: [Index(nameof(MakeId), Name = "IX_Inventory_MakeId")]
Class Attributes:  
- Table attribute: `[Table("Inventory", Schema = "dbo")]`
  
Properties: 
- Color (string, attributes: required, max length 50)
- Price (string)
- IsDrivable (bool, initialized to true, DisplayName = "Is Drivable")
- DateBuilt (DateTime?)
- Display (string, private set, attributes: Database Generated and Computed)
- PetName (string, required, max length 50, DisplayName = "Pet Name")
- MakeId (int, required, DisplayName = "Make")

## Class Name: CarDriver
Location: Entities  
Inherits from: BaseEntity  
Class Attributes:  
- Table attribute: `[Table("InventoryToDrivers", Schema = "dbo")]`
  
Properties: 
- DriverId (int)
- CarId (int, Column = "InventoryId")

## Class Name: Driver
Location: Entities  
Inherits from: BaseEntity  
Class Attributes:  
- Table attribute: `[Table("Drivers", Schema = "dbo")]`

Properties: 
- PersonInformation (Person, initialized to new Person)

## Class Name: Make
Location: Entities  
Inherits from: BaseEntity  
Class Attributes:  
- Table attribute: `[Table("Makes", Schema = "dbo")]`
  
Properties: 
- Name (string, required, max length 50)

## Class Name: Radio
Location: Entities  
Inherits from: BaseEntity  
Class Attributes:  
- Table attribute: `[Table("Radios", Schema = "dbo")]`
  
Properties: 
- HasTweeters (bool)
- HasSubWoofers (bool)
- RadioId (string, attributes: required, max length 50)
- CarId (int, attributes: column name "InventoryId")

## Class Name: SeriLogEntry
Location: Entities
Class Attributes:  
- Table attribute: `[Table("SeriLogs", Schema = "Logging")]`
  
Properties: 
- Id (int, Key, Database Generated Identity)
- Message, 
- MessageTemplate (string)
- Level (string, max length 128)
- TimeStamp (DateTime)
- Exception (string)
- Properties (string)
- LogEvent (string)
- SourceContext (string)
- RequestPath (string)
- ActionName (string)
- ApplicationName (string)
- MachineName (string)
- FilePath (string)
- MemberName (string)
- LineNumber (nullable int)
- PropertiesXml (XElement, not mapped, readonly property with value of: Properties != null ? XElement.Parse(Properties) : null)

# Create View Model Classes

## Class Name: CarViewModel
Location: ViewModels
Class Attributes:
- Keyless

Properties:
- Id (int)
- IsDrivable (bool)
- DateBuilt (DateTime?)
- Price (string)
- MakeId (int)
- Color (string, max length 50)
- PetName (string, max length 50)

# Update the Global Usings File
Add all namespaces for the created entities to the GlobalUsings.cs file:
```csharp
global using AutoLot.Models.Entities;
global using AutoLot.Models.Entities.Base;
global using AutoLot.Models.Entities.ComplexTypes;
global using AutoLot.Models.ViewModels;
```

# Build Confirmation
Confirm the project builds successfully before continuing.

# Navigation Properties

## Naming Conventions
- **Instance navigation properties** (many-to-one or one-to-one) should be **singular** with a suffix of Navigation (e.g., `MakeNavigation`, `CarNavigation`, `DriverNavigation`)
- **Collection navigation properties** (one-to-many) should be **plural** (e.g., `Cars`, `CarDrivers`)
- All navigation properties must have `[InverseProperty(nameof(...))]` attributes to specify the inverse relationship and maintain bidirectional mapping integrity
- Per the C# instructions, use `ICollection<T>` for collection navigation properties and always initialize them to `new List<T>()`

## Add Navigation Properties to Entity Classes

### Make Entity
- Property: `ICollection<Car> Cars { get; set; } = new List<Car>();` (one-to-many)
- Attribute: `[InverseProperty(nameof(Car.MakeNavigation))]`

### Car Entity
- Property: `Make MakeNavigation { get; set; }` (many-to-one)
  - Attributes: `[ForeignKey(nameof(MakeId))]`, `[InverseProperty(nameof(Make.Cars))]`
- Property: `Radio RadioNavigation { get; set; }` (one-to-one, Car is parent)
  - Attribute: `[InverseProperty(nameof(Radio.CarNavigation))]`
- Property: `ICollection<CarDriver> CarDrivers { get; set; } = new List<CarDriver>();` (one-to-many)
  - Attribute: `[InverseProperty(nameof(CarDriver.CarNavigation))]`
- Property: `ICollection<Driver> Drivers { get; set; } = new List<Driver>();` (many-to-many)
  - Attribute: `[InverseProperty(nameof(Driver.Cars))]`

### Radio Entity
- Property: `Car CarNavigation { get; set; }` (one-to-one, Car is parent)
  - Attributes: `[ForeignKey(nameof(CarId))]`, `[InverseProperty(nameof(Car.RadioNavigation))]`

### CarDriver Entity
- Property: `Car CarNavigation { get; set; }` (many-to-one)
  - Attributes: `[ForeignKey(nameof(CarId))]`, `[InverseProperty(nameof(Car.CarDrivers))]`
- Property: `Driver DriverNavigation { get; set; }` (many-to-one)
  - Attributes: `[ForeignKey(nameof(DriverId))]`, `[InverseProperty(nameof(Driver.CarDrivers))]`

### Driver Entity
- Property: `ICollection<CarDriver> CarDrivers { get; set; } = new List<CarDriver>();` (one-to-many)
  - Attribute: `[InverseProperty(nameof(CarDriver.DriverNavigation))]`

# Update Car Entity
Override the `ToString()` method in the `Car` entity to return a string in the following format:
```csharp
public override string ToString() => $"{PetName ?? "**No Name**"} is a {Color} {MakeNavigation?.Name} with ID {Id}.";
```

Add a NotMapped property to the `Car` entity called `MakeName` that returns a string in the following format:
```csharp
[NotMapped]
public string MakeName => MakeNavigation?.Name ?? "Unknown";
```