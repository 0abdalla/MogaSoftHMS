using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core._Data.Configurations;
internal class CostCenterConfiguration : IEntityTypeConfiguration<CostCenterTree>
{
    public void Configure(EntityTypeBuilder<CostCenterTree> builder)
    {
        builder.ToTable("CostCenters", "finance");

        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.NameAR)
            .IsRequired()
            .HasMaxLength(100);    
    }
}
