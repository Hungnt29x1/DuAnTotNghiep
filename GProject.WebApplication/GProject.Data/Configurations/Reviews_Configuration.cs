using GProject.Data.DomainClass;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.Configurations
{
    public class Reviews_Configuration : IEntityTypeConfiguration<Reviews>
    {
        public void Configure(EntityTypeBuilder<Reviews> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.HasOne(d => d.CustomerId_Navigation).WithMany(p => p.Reviews).HasForeignKey(d => d.AccountID);
            builder.HasOne(d => d.PostId_Navigation).WithMany(p => p.Reviews).HasForeignKey(d => d.PostId);
        }
    }
}
