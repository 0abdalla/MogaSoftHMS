using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core._Data.Configurations;
public class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.ToTable("Banks", schema: "finance");

        builder.HasKey(x => x.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200);


        builder.Property(b => b.IsActive)
            .HasDefaultValue(true);
    }
}