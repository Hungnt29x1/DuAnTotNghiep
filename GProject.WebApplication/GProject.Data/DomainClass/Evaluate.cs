using GProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.DomainClass
{
    public class Evaluate
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? CustomerId { get; set; }
        public string? Comment { get; set; }
        public int? Rating { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Product? ProductId_Navigation { get; set; }
        public Customer? CustomerId_Navigation { get; set; }
    }
}
