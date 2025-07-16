using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations;

public class TreasuryOperationsConfiguration : IEntityTypeConfiguration<TreasuryOperation>
{
    public void Configure(EntityTypeBuilder<TreasuryOperation> builder)
    {
        builder.ToTable("TreasuryOperations", "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DocumentNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ReceivedFrom)
            .HasMaxLength(250);

        builder.Property(x => x.Amount)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);        
    }
}