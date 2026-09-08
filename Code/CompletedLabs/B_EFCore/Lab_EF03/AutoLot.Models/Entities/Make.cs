// Copyright Information
// ==================================
// AutoLot - AutoLot.Models - Make.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2026/09/05
// ==================================

namespace AutoLot.Models.Entities;

[Table(
    "Makes",
    Schema = "dbo")]
[EntityTypeConfiguration(typeof(MakeConfiguration))]
public class Make : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [InverseProperty(nameof(Car.MakeNavigation))]
    public ICollection<Car> Cars { get; set; } = new List<Car>();
}