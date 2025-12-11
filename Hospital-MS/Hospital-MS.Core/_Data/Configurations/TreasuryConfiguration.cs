using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations;

public class TreasuryConfiguration : IEntityTypeConfiguration<Treasury>
{
    public void Configure(EntityTypeBuilder<Treasury> builder)
    {
        builder.ToTable("Treasuries", schema: "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountId)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(50);

    }
}