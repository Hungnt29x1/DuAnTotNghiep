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
    public class Size_Configuration : IEntityTypeConfiguration<Size>
    {
        public void Configure(EntityTypeBuilder<Size> builder)
        {
            builder.ToTable("Size");
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(e => e.Name).HasMaxLength(200);
            builder.Property(e => e.Code).HasMaxLength(5);
            builder.Property(e => e.Status).HasDefaultValueSql("((1))");
        }
    }
}
