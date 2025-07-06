using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core._Data.Configurations;
public class DailyRestrictionDetailConfiguration : IEntityTypeConfiguration<DailyRestrictionDetail>
{
    public void Configure(EntityTypeBuilder<DailyRestrictionDetail> builder)
    {
        builder.ToTable("DailyRestrictionDetails", "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Debit)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Credit)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Note)
            .HasMaxLength(250);
    }
}