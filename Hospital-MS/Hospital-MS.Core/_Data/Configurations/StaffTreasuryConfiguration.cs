using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core._Data.Configurations;
internal class StaffTreasuryConfiguration : IEntityTypeConfiguration<StaffTreasury>
{
    public void Configure(EntityTypeBuilder<StaffTreasury> builder)
    {
        builder.ToTable("StaffTreasuries", schema: "finance");

        builder.HasKey(x => new { x.StaffId, x.TreasuryId });

        builder.HasOne(x => x.Staff)
            .WithMany()
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Treasury)
            .WithMany()
            .HasForeignKey(x => x.TreasuryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

