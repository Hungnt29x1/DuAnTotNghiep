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
    public class DeliveryAddress_Configuration : IEntityTypeConfiguration<DeliveryAddress>
    {
        public void Configure(EntityTypeBuilder<DeliveryAddress> builder)
        {
            builder.ToTable("DeliveryAddress");

            builder.Property(x => x.Name).HasMaxLength(225).IsRequired();
            builder.Property(x=>x.ProvinceName).HasMaxLength(225).IsRequired();
            builder.Property(x => x.DistrictName).HasMaxLength(225).IsRequired();
            builder.Property(x => x.WardName).HasMaxLength(225).IsRequired();
            builder.Property(x => x.Address).HasMaxLength(225).IsRequired();
            builder.Property(x => x.PhoneNumber).HasMaxLength(225).IsRequired();

            builder.Property(x => x.ProvinceID).IsRequired();
            builder.Property(x => x.DistrictID).IsRequired();
            builder.Property(x => x.WardCode).HasMaxLength(225).IsRequired();
        }
    }
}
