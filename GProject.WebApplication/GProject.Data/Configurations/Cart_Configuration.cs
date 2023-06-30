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
    public class Cart_Configuration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart");
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.CartId).IsUnicode(false).HasMaxLength(15);
            builder.Property(e => e.Status).HasDefaultValueSql("((0))");
            builder.HasOne(d => d.CustomerId_Navigation).WithMany(p => p.Carts).HasForeignKey(d => d.CustomerId);
        }
    }
}
