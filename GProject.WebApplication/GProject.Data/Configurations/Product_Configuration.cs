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
    public class Product_Configuration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.BrandId).HasMaxLength(15);
            builder.Property(e => e.ProductCode).HasMaxLength(120);
            builder.Property(e => e.Name).HasMaxLength(200);
            builder.Property(e => e.ViewCount).HasDefaultValueSql("((0))");
            builder.Property(e => e.LikeCount).HasDefaultValueSql("((0))");
            builder.Property(e => e.Price).HasDefaultValueSql("((0))");
            builder.Property(e => e.ImportPrice).HasDefaultValueSql("((0))");
            builder.Property(e => e.Status).HasDefaultValueSql("((0))");
            builder.HasOne(d => d.BrandId_Navigation).WithMany(p => p.Products).HasForeignKey(d => d.BrandId);
            builder.HasOne(d => d.CategoryId_Navigation).WithMany(p => p.Products).HasForeignKey(d => d.CategoryId);
        }
    }
}
