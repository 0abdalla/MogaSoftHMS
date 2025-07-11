using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations;

public class RestrictionTypeConfiguration : IEntityTypeConfiguration<RestrictionType>
{
    public void Configure(EntityTypeBuilder<RestrictionType> builder)
    {
        builder.ToTable("RestrictionTypes", "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(250);
    }
}