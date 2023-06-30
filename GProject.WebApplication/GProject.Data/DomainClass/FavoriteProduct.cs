using GProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.DomainClass
{
    public class FavoriteProduct
    {
        public Guid? ProductId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime CreateDate { get; set; }
        public Product? ProductId_Navigation { get; set; }
        public Customer? CustomerId_Navigation { get; set; }
    }
}
