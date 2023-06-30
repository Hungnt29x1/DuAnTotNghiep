using GProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.DomainClass
{
    public class ProductVariation
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? Image { get; set; }
        public Product? ProductId_Navigation { get; set; }
        public Color? ColorId_Navigation { get; set; }
        public Size? SizeId_Navigation { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
        public List<CartDetail>? CartDetails { get; set; }
    }
}
