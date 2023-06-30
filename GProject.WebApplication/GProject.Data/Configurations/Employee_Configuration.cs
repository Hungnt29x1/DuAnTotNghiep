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
    public class Employee_Configuration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.EmployeeId).IsUnicode(false).HasMaxLength(15);
            builder.Property(e => e.Name).HasMaxLength(50);
            builder.Property(e => e.DateOfBirth).HasColumnType("date");
            builder.Property(e => e.DateOfJoin).HasColumnType("date");
            builder.Property(e => e.Email).HasMaxLength(100);
            builder.Property(e => e.Password).HasMaxLength(100);
            builder.Property(e => e.PersonalId).HasMaxLength(15);
            builder.Property(e => e.PhoneNumber).HasMaxLength(15);
            builder.Property(e => e.Status).HasDefaultValueSql("((0))");
            builder.Property(e => e.Position).HasDefaultValueSql("((0))");
        }
    }
}
