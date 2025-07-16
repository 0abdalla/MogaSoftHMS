using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations;

public class DispensePermissionConfiguration : IEntityTypeConfiguration<DispensePermission>
{
    public void Configure(EntityTypeBuilder<DispensePermission> builder)
    {
        builder.ToTable("DispensePermissions", schema: "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(500)
            .IsRequired(false);


        builder.Property(x => x.DispenseTo)
            .HasMaxLength(250)
            .IsRequired(false);
    }
}