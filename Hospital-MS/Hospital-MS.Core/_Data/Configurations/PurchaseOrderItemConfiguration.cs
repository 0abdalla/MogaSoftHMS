using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core._Data.Configurations;
public class PurchaseOrderItemConfiguration : IEntityTypeConfiguration<PurchaseOrderItem>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
    {
        builder.ToTable("PurchaseOrderItems", schema: "finance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Unit).HasMaxLength(50);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.HasOne(x => x.PurchaseOrder).WithMany(x => x.Items).HasForeignKey(x => x.PurchaseOrderId);
        builder.HasOne(x => x.Item).WithMany().HasForeignKey(x => x.ItemId);
    }
}