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
    public class Contact_Configuration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contact");
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Username).HasMaxLength(50);
            builder.Property(e => e.Message);
            builder.Property(e => e.Status).HasDefaultValueSql("((1))");
            builder.HasOne(d => d.CustomerId_navigation).WithMany(p => p.Contacts).HasForeignKey(d => d.CustomerId);
            builder.HasOne(d => d.EmployeeId_navigation).WithMany(p => p.Contacts).HasForeignKey(d => d.EmployeeId);
        }
    }
}
