// Copyright Information
// ==================================
// AutoLot - AutoLot.Dal - ApplicationDbContext.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2026/09/05
// ==================================

namespace AutoLot.Dal.EfStructures;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarDriver> CarDrivers { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Make> Makes { get; set; }
    public DbSet<Radio> Radios { get; set; }
    public DbSet<SeriLogEntry> SeriLogEntries { get; set; }
    public DbSet<CarViewModel> CarViewModels { get; set; }


    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations from AutoLot.Models.Entities.Configuration
        modelBuilder.ApplyConfiguration(new CarConfiguration());
        modelBuilder.ApplyConfiguration(new DriverConfiguration());
        modelBuilder.ApplyConfiguration(new MakeConfiguration());
        modelBuilder.ApplyConfiguration(new RadioConfiguration());
        modelBuilder.ApplyConfiguration(new CarDriverConfiguration());
        modelBuilder.ApplyConfiguration(new SeriLogEntryConfiguration());

        // Apply configurations from AutoLot.Models.ViewModels.Configuration
        modelBuilder.ApplyConfiguration(new CarViewModelConfiguration());
    }
}