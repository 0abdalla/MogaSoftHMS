﻿using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core._Data.Configurations;
public class MaterialIssueItemConfiguration : IEntityTypeConfiguration<MaterialIssueItem>
{
    public void Configure(EntityTypeBuilder<MaterialIssueItem> builder)
    {
        builder.ToTable("MaterialIssueItems", "finance");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Unit).HasMaxLength(50);
        builder.Property(x => x.Quantity).HasColumnType("decimal(18,2)");
        builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
    }
}