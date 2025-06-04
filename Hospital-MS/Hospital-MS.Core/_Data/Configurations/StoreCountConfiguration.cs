using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations;

public class StoreCountConfiguration : IEntityTypeConfiguration<StoreCount>
{
    public void Configure(EntityTypeBuilder<StoreCount> builder)
    {
        builder.ToTable("StoreCounts", schema: "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FromDate)
            .IsRequired();

        builder.Property(x => x.ToDate)
            .IsRequired();
    }
}