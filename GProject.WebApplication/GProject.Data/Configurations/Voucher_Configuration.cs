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
    public class Voucher_Configuration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("Vouchers");
            builder.Property(x => x.Name).HasMaxLength(225).IsRequired();
            builder.Property(x => x.VoucherId).HasMaxLength(125).IsRequired();
            builder.Property(x => x.DiscountForm).HasMaxLength(125).IsRequired();
        }
    }
}
