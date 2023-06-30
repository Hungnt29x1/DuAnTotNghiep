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
    public class SendMail_Configuration : IEntityTypeConfiguration<SendMail>
    {
        public void Configure(EntityTypeBuilder<SendMail> builder)
        {
            builder.ToTable("SendMail");
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Sender).HasMaxLength(100);
            builder.Property(e => e.Subject).HasMaxLength(100);
            builder.Property(e => e.FileName).HasMaxLength(100);
        }
    }
}
