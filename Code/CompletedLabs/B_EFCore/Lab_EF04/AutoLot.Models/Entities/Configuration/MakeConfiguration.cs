// Copyright Information
// ==================================
// AutoLot - AutoLot.Models - MakeConfiguration.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2026/09/05
// ==================================

namespace AutoLot.Models.Entities.Configuration;

public class MakeConfiguration : IEntityTypeConfiguration<Make>
{
    public void Configure(
        EntityTypeBuilder<Make> builder)
    {
        builder.Property(e => e.TimeStamp)
            .IsRowVersion()
            .HasConversion<byte[]>();
    }
}