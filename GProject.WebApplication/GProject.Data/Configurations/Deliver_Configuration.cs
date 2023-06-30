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
    public class Deliver_Configuration : IEntityTypeConfiguration<Deliver>
    {
        public void Configure(EntityTypeBuilder<Deliver> builder)
        {
            builder.ToTable("Deliver");
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.DeliverId).IsUnicode(false).HasMaxLength(15);
            builder.Property(e => e.Name).HasMaxLength(50);
            builder.Property(e => e.DateOfBirth).HasColumnType("date");
            builder.Property(e => e.DateOfJoin).HasColumnType("date");
            builder.Property(e => e.Email).HasMaxLength(100);
            builder.Property(e => e.Password).HasMaxLength(100);
            builder.Property(e => e.PersonalId).HasMaxLength(15);
            builder.Property(e => e.PhoneNumber).HasMaxLength(15);
            builder.Property(e => e.LicensePlate).HasMaxLength(50);
            builder.Property(e => e.BrandCar).HasMaxLength(64);
            builder.Property(e => e.DrivingLicense).HasMaxLength(64);
            builder.Property(e => e.DrivingNumber).HasMaxLength(64);
            builder.Property(e => e.Status).HasDefaultValueSql("((0))");
        }
    }
}
