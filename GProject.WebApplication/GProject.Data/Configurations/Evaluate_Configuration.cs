using GProject.Data.DomainClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.Configurations
{
    public class Evaluate_Configuration : IEntityTypeConfiguration<Evaluate>
    {
        public void Configure(EntityTypeBuilder<Evaluate> builder)
        {
            builder.ToTable("Evaluate");
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.HasOne(d => d.ProductId_Navigation).WithMany(p => p.Evaluates).HasForeignKey(d => d.ProductId);
            builder.HasOne(d => d.CustomerId_Navigation).WithMany(p => p.Evaluates).HasForeignKey(d => d.CustomerId);
        }
    }
}
