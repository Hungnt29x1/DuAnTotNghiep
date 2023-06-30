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
    public class PromotionDetail
    {
        public Guid? PromotionId { get; set; }
        public Guid? ProductId { get; set; }
        [Column(TypeName = "decimal(20, 0)")]
        public decimal InitialPrice { get; set; }
        [Column(TypeName = "decimal(20, 0)")]
        public decimal CurrentPrice { get; set; }
        public int Status { get; set; } // 0: không áp dụng || 1: đang áp dụng
        public Promotion? PromotionId_Navidation { get; set; }
        public Product? ProductId_Navigation { get; set; }
    
    }
}
