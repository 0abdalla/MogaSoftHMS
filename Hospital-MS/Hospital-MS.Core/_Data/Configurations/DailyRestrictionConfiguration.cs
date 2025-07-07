using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core._Data.Configurations;
public class DailyRestrictionConfiguration : IEntityTypeConfiguration<DailyRestriction>
{
    public void Configure(EntityTypeBuilder<DailyRestriction> builder)
    {
        builder.ToTable("DailyRestrictions", "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RestrictionNumber)
            .HasMaxLength(50);

        builder.Property(x => x.RestrictionDate)
            .IsRequired();

        builder.Property(x => x.LedgerNumber)
            .HasMaxLength(50);

        builder.Property(x => x.Description)
            .HasMaxLength(500);
    }
}
