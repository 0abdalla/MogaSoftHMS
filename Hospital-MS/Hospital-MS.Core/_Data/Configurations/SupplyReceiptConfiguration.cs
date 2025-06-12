using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations;

public class SupplyReceiptConfiguration : IEntityTypeConfiguration<SupplyReceipt>
{
    public void Configure(EntityTypeBuilder<SupplyReceipt> builder)
    {
        builder.ToTable("SupplyReceipts", schema: "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.ReceivedFrom)
            .HasMaxLength(100);

        builder.Property(x => x.AccountCode)
            .HasMaxLength(50);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);
    }
}