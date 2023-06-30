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
    public class CartDetail
    {
        public Guid? CartId { get; set; }
        public Guid? ProductVariationId { get; set; }
        [Column(TypeName = "decimal(20, 0)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(20, 0)")]
        public decimal ToatlMoney { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? TimeOut { get; set; }
        public CartDetailStatus Status { get; set; }
        public string Description { get; set; }
        public Cart? CartId_Navigation { get; set; }
        public ProductVariation? ProductVariationId_Navigation { get; set; }
    }
}
