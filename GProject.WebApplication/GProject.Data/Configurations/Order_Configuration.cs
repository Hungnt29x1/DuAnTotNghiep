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
    public class Order_Configuration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.OrderId).IsUnicode(false).HasMaxLength(15);
            builder.Property(e => e.ShippingFullName).HasMaxLength(100);
            builder.Property(e => e.ShippingCountry).HasMaxLength(100);
            builder.Property(e => e.ShippingCity).HasMaxLength(100);
            builder.Property(e => e.ShippingDistrict).HasMaxLength(100);
            builder.Property(e => e.ShippingTown).HasMaxLength(100);
            builder.Property(e => e.ShippingPhone).HasMaxLength(15);
            builder.Property(e => e.ShippingEmail).HasMaxLength(200);
            builder.Property(e => e.Status).HasDefaultValueSql("((0))");
            builder.Property(e => e.ShippingFee).HasDefaultValueSql("((0))");
            builder.Property(e => e.VoucherId).HasDefaultValueSql("((0))");
            builder.Property(e => e.DiscountRate).HasDefaultValueSql("((0))");
            builder.HasOne(d => d.CustomerId_Nagavition).WithMany(p => p.Orders).HasForeignKey(d => d.CustomerId);
            //builder.HasOne(d => d.EmployeeId_Nagavition).WithMany(p => p.Orders).HasForeignKey(d => d.EmployeeId);
            //builder.HasOne(d => d.DeliverId_Nagavition).WithMany(p => p.Orders).HasForeignKey(d => d.DeliverId);
        }
    }
}
