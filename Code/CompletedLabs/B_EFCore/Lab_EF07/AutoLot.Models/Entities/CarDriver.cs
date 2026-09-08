// Copyright Information
// ==================================
// AutoLot - AutoLot.Models - CarDriver.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2026/09/06
// ==================================

namespace AutoLot.Models.Entities;

[Table(
    "InventoryToDrivers",
    Schema = "dbo")]
[EntityTypeConfiguration(typeof(CarDriverConfiguration))]
public class CarDriver : BaseEntity
{
    public int DriverId { get; set; }

    [Column("InventoryId")]
    public int CarId { get; set; }

    [ForeignKey(nameof(CarId))]
    [InverseProperty(nameof(Car.CarDrivers))]
    public Car CarNavigation { get; set; }

    [ForeignKey(nameof(DriverId))]
    [InverseProperty(nameof(Driver.CarDrivers))]
    public Driver DriverNavigation { get; set; }
}