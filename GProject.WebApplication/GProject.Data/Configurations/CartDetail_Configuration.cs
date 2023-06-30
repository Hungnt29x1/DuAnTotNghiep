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
    public class CartDetail_Configuration : IEntityTypeConfiguration<CartDetail>
    {
        public void Configure(EntityTypeBuilder<CartDetail> builder)
        {
            builder.ToTable("CartDetail");
            builder.HasKey(e => new { e.CartId, e.ProductVariationId });
            builder.Property(e => e.Price).HasDefaultValueSql("((0))");
            builder.Property(e => e.Quantity).HasDefaultValueSql("((0))");
            builder.Property(e => e.Status).HasDefaultValueSql("((0))");
            builder.HasOne(d => d.ProductVariationId_Navigation).WithMany(p => p.CartDetails).HasForeignKey(d => d.ProductVariationId);
            builder.HasOne(d => d.CartId_Navigation).WithMany(p => p.CartDetails).HasForeignKey(d => d.CartId);
        }
    }
}
