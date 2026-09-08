## Add the Configurations
You are a C# developer that specializes in Entity Framework Core.

Make sure to read and ingest the general instructions file (.github/copilot-instructions.md) and the C# instructions (.github/instructions/csharp.instructions.md).
This entire lab takes place in the AutoLot.Models Project.

# Create the Configuration Classes

## Class Name: CarConfiguration
Location: Entities/Configuration  
Derives from: IEntityTypeConfiguration<Car>  
Public properties:  
- public const string IsNewQueryFilterName = "IsNew";
- public const string IsDriveableFilterName = "IsDriveable";
Overridden Configure method:
```csharp
	public void Configure(EntityTypeBuilder<Car> builder)
	{
        builder.Property(e => e.TimeStamp)
            .IsRowVersion()
            .HasConversion<byte[]>();
        builder.HasQueryFilter(IsDriveableFilterName, c => c.IsDrivable);
        builder.HasQueryFilter(IsNewQueryFilterName, c => c.DateBuilt > new DateTime(2020, 1, 1));
        builder.Property(p => p.IsDrivable).HasDefaultValue(true);
        builder.Property(e => e.DateBuilt).HasDefaultValueSql("getdate()");
        builder.Property(e => e.Display)
            .HasComputedColumnSql("[PetName] + ' (' + [Color] + ')'", stored: true);
        CultureInfo provider = new("en-us");
        NumberStyles style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
        builder.Property(p => p.Price).HasConversion(
            v => string.IsNullOrEmpty(v) ? (decimal?)null : decimal.Parse(v, style, provider),
            v => v.HasValue ? v.Value.ToString("C2", provider) : null);
        builder.HasOne(d => d.MakeNavigation).WithMany(p => p.Cars).HasForeignKey(d => d.MakeId)
            .OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Inventory_Makes_MakeId");
        builder.HasMany(p => p.Drivers).WithMany(p => p.Cars).UsingEntity<CarDriver>(
            j => j.HasOne(cd => cd.DriverNavigation).WithMany(d => d.CarDrivers)
                .HasForeignKey(nameof(CarDriver.DriverId))
                .HasConstraintName("FK_InventoryDriver_Drivers_DriverId")
                .OnDelete(DeleteBehavior.Cascade),
            j => j.HasOne(cd => cd.CarNavigation).WithMany(c => c.CarDrivers)
                .HasForeignKey(nameof(CarDriver.CarId))
                .HasConstraintName("FK_InventoryDriver_Inventory_InventoryId")
                .OnDelete(DeleteBehavior.ClientCascade),
            j => { j.HasKey(x => x.Id); });
	}
```

## Class Name: CarDriverConfiguration
Location: Entities/Configuration  
Derives from: IEntityTypeConfiguration<CarDriver>  
Overridden Configure method:
```csharp
	public void Configure(EntityTypeBuilder<CarDriver> builder)
	{
        builder.HasIndex(e => new { e.DriverId, e.CarId })
            .IsUnique()
            .HasDatabaseName("IX_InventoryToDrivers_DriverId_CarId");
        builder.Property(e => e.TimeStamp)
            .IsRowVersion()
            .HasConversion<byte[]>();
        builder.HasOne(e => e.DriverNavigation)
            .WithMany(d => d.CarDrivers)
            .HasForeignKey(e => e.DriverId);
        builder.HasOne(e => e.CarNavigation)
            .WithMany(c => c.CarDrivers)
            .HasForeignKey(e => e.CarId);
        builder.HasQueryFilter(CarConfiguration.IsDriveableFilterName, c => c.CarNavigation.IsDrivable);
        builder.HasQueryFilter(CarConfiguration.IsNewQueryFilterName,
            c => c.CarNavigation.DateBuilt > new DateTime(2020, 1, 1));
	}
```

## Class Name: DriverConfiguration
Location: Entities/Configuration  
Derives from: IEntityTypeConfiguration<Driver>  
Overridden Configure method:
```csharp

	public void Configure(EntityTypeBuilder<Driver> builder)
	{
        builder.Property(e => e.TimeStamp)
            .IsRowVersion()
            .HasConversion<byte[]>();

        builder.ComplexProperty(cp => cp.PersonInformation,
            pd =>
            {
                pd.Property<string>(nameof(Person.FirstName))
                    .HasColumnName(nameof(Person.FirstName))
                    .HasColumnType("nvarchar(50)");
                pd.Property<string>(nameof(Person.LastName))
                    .HasColumnName(nameof(Person.LastName))
                    .HasColumnType("nvarchar(50)");
                pd.Property(p => p.FullName)
                    .HasColumnName(nameof(Person.FullName))
                    .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");
                pd.IsRequired(true);
            });
	}
```

## Class Name: MakeConfiguration
Location: Entities/Configuration  
Derives from: IEntityTypeConfiguration<Make>  
Overridden Configure method:
```csharp

	public void Configure(EntityTypeBuilder<Make> builder)
	{
        builder.Property(e => e.TimeStamp)
            .IsRowVersion()
            .HasConversion<byte[]>();
	}
```

## Class Name: RadioConfiguration
Location: Entities/Configuration  
Derives from: IEntityTypeConfiguration<Radio>  
Overridden Configure method:
```csharp

	public void Configure(EntityTypeBuilder<Radio> builder)
	{
        builder.Property(e => e.TimeStamp)
            .IsRowVersion()
            .HasConversion<byte[]>();
        builder.HasQueryFilter(e => e.CarNavigation.IsDrivable);
        builder.HasOne(d => d.CarNavigation)
            .WithOne(p => p.RadioNavigation)
            .HasForeignKey<Radio>(d => d.CarId);
	}
```

## Class Name: SeriLogEntryConfiguration
Location: Entities/Configuration  
Derives from: IEntityTypeConfiguration<SeriLogEntry>  
Overridden Configure method:
```csharp

	public void Configure(EntityTypeBuilder<SeriLogEntry> builder)
	{
        builder.Property(e => e.Properties).HasColumnType("Xml");
        builder.Property(e => e.TimeStamp).HasDefaultValueSql("GetDate()");
        builder.Property(p => p.LineNumber).HasDefaultValue(0).HasSentinel(-1);
	}
```

## Class Name: CarViewModelConfiguration
Location: ViewModels/Configuration  
Derives from: IEntityTypeConfiguration<CarViewModel>  
Overridden Configure method:
```csharp

	public void Configure(EntityTypeBuilder<CarViewModel> builder)
	{
        builder.ToTable(t => t.ExcludeFromMigrations());
        CultureInfo provider = new("en-us");
        NumberStyles style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
        builder.Property(p => p.Price).HasConversion(
            v => string.IsNullOrEmpty(v) ? (decimal?)null : decimal.Parse(v, style, provider),
            v => v.HasValue ? v.Value.ToString("C2", provider) : null);
	}
```
# Update the Global Usings File
Add the following to the "GlobalUsings.cs" file:
```csharp
global using AutoLot.Models.Entities.Configuration;
global using AutoLot.Models.ViewModels.Configuration;
```
# Update the Models
Add the [EntityTypeConfiguration(typeof(<configuration class name>))] to all of the entities and view models.  For example, the Car class should look like this:
```csharp
[EntityTypeConfiguration(typeof(CarConfiguration))]
```