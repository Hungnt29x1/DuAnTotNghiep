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
    public class Posts_Configuration : IEntityTypeConfiguration<Posts>
    {
        public void Configure(EntityTypeBuilder<Posts> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.HasOne(d => d.EmployeeId_Navigation).WithMany(p => p.Posts).HasForeignKey(d => d.EmployeeId);
            builder.HasOne(d => d.CategoryId_Navigation).WithMany(p => p.Posts).HasForeignKey(d => d.CategoryId);
        }
    }
}
