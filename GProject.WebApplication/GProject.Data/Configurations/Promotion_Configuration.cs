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
    public class Promotion_Configuration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.ToTable("Promotion");
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.PromotionId).IsUnicode(false).HasMaxLength(15);
            builder.Property(e => e.DiscountPercent).HasDefaultValueSql("((0))");
            builder.Property(e => e.DiscountRate).HasDefaultValueSql("((0))");
            builder.Property(e => e.Status).HasDefaultValueSql("((0))");
        }
    }
}
