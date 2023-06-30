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
    public class OrderDetail_Configuration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetail");
            builder.HasKey(e => new { e.OrderId, e.ProductVariationId });
            builder.Property(e => e.Price).HasDefaultValueSql("((0))");
            builder.Property(e => e.Quantity).HasDefaultValueSql("((0))");
            builder.Property(e => e.Status).HasDefaultValueSql("((0))");
            builder.HasOne(d => d.ProductVariationId_Navigation).WithMany(p => p.OrderDetails).HasForeignKey(d => d.ProductVariationId);
            builder.HasOne(d => d.OrderId_Navigation).WithMany(p => p.OrderDetails).HasForeignKey(d => d.OrderId);
        }
    }
}
