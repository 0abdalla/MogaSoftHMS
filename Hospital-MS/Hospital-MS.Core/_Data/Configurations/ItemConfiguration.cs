using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items", schema: "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.NameAr)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.NameEn)
            .HasMaxLength(100);

        builder.Property(x => x.UnitId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.OrderLimit)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property(x => x.Cost)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property(x => x.OpeningBalance)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property(x => x.SalesTax)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property(x => x.Price)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property(x => x.PriceAfterTax)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);
    }
}