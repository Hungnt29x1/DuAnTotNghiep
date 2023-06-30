using GProject.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.DomainClass
{
    public class OrderDetail
    {
        public Guid? OrderId { get; set; }
        public Guid? ProductVariationId { get; set; }
        [Column(TypeName = "decimal(20, 0)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(20, 0)")]
        public decimal TotalMoney { get; set; }
        public OrderDetailStatus Status { get; set; }
        public Order? OrderId_Navigation { get; set; }
        public ProductVariation? ProductVariationId_Navigation { get; set; }
    }
}
