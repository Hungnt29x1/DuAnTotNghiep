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
    public class Cart
    {
        public Guid? Id { get; set; }
        public Guid? CustomerId { get; set; }
        public string CartId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Status { get; set; } = 0;
        public string? Description { get; set; }
        public Customer? CustomerId_Navigation { get; set; }
        public List<CartDetail>? CartDetails { get; set; }
    }
}
