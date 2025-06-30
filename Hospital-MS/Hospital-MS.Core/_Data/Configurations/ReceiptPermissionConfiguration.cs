using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core._Data.Configurations;
public class ReceiptPermissionConfiguration : IEntityTypeConfiguration<ReceiptPermission>
{
    public void Configure(EntityTypeBuilder<ReceiptPermission> builder)
    {
        builder.ToTable("ReceiptPermissions", schema: "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PermissionNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.DocumentNumber)
           .IsRequired()
           .HasMaxLength(50);

        builder.Property(x => x.PermissionDate)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(500);
    }
}
