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
    public class ViewHistory_Configuration : IEntityTypeConfiguration<ViewHistory>
    {
        public void Configure(EntityTypeBuilder<ViewHistory> builder)
        {
            builder.ToTable("ViewHistory");
            builder.HasKey(t => new { t.CustomerId, t.ProductId });
            builder.HasOne(d => d.ProductId_Navigation).WithMany(p => p.ViewHistories).HasForeignKey(d => d.ProductId);
            builder.HasOne(d => d.CustomerId_Navigation).WithMany(p => p.ViewHistories).HasForeignKey(d => d.CustomerId);
        }
    }
}
