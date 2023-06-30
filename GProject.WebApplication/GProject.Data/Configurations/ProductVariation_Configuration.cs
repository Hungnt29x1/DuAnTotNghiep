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
    public class ProductVariation_Configuration : IEntityTypeConfiguration<ProductVariation>
    {
        public void Configure(EntityTypeBuilder<ProductVariation> builder)
        {
            builder.ToTable("ProductVariation");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.QuantityInStock).HasDefaultValueSql("((0))");
            builder.HasOne(d => d.ProductId_Navigation).WithMany(p => p.ProductVariations).HasForeignKey(d => d.ProductId);
            builder.HasOne(d => d.ColorId_Navigation).WithMany(p => p.Variations).HasForeignKey(d => d.ColorId);
            builder.HasOne(d => d.SizeId_Navigation).WithMany(p => p.Variations).HasForeignKey(d => d.SizeId);
        }
    }
}
