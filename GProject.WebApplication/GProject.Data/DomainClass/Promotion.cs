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
    public class Promotion
    {
        public Guid? Id { get; set; }
        public string PromotionId { get; set; }
        public string PromotionName { get; set; }
        public int DiscountPercent { get; set; } // 0: theo Vnd || 1: theo %
        [Column(TypeName = "decimal(20, 0)")]
        public decimal DiscountRate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Image { get; set; }
        public string CategoryApply { get; set; }
        public string ProductApply { get; set; }
        public string CreateBy { get; set; }
        public PromotionStatus Status { get; set; }
        public string? Description { get; set; }
        public List<PromotionDetail>? PromotionDetails { get; set; }
    }
}
