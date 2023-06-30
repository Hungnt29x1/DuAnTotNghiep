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
    public class FavoriteProduct_Configuration : IEntityTypeConfiguration<FavoriteProduct>
    {
        public void Configure(EntityTypeBuilder<FavoriteProduct> builder)
        {
            builder.ToTable("FavoriteProduct");
            builder.HasKey(t => new { t.CustomerId, t.ProductId });
            builder.HasOne(d => d.ProductId_Navigation).WithMany(p => p.FavoriteProducts).HasForeignKey(d => d.ProductId);
            builder.HasOne(d => d.CustomerId_Navigation).WithMany(p => p.FavoriteProducts).HasForeignKey(d => d.CustomerId);
        }
    }
}
