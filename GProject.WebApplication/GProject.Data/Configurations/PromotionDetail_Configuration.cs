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
    public class PromotionDetail_Configuration : IEntityTypeConfiguration<PromotionDetail>
    {
        public void Configure(EntityTypeBuilder<PromotionDetail> builder)
        {
            builder.ToTable("PromotionDetail");
            builder.HasKey(e => new { e.PromotionId, e.ProductId });
            builder.Property(e => e.InitialPrice).HasDefaultValueSql("((0))");
            builder.Property(e => e.CurrentPrice).HasDefaultValueSql("((0))");
            builder.Property(e => e.Status).HasDefaultValueSql("((0))");
            builder.HasOne(d => d.ProductId_Navigation).WithMany(p => p.PromotionDetails).HasForeignKey(d => d.ProductId);
            builder.HasOne(d => d.PromotionId_Navidation).WithMany(p => p.PromotionDetails).HasForeignKey(d => d.PromotionId);
        }
    }
}
