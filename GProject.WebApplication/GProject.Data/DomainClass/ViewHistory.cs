using GProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.DomainClass
{
    public class ViewHistory
    {
        public Guid? ProductId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime DateView { get; set; }
        public Product? ProductId_Navigation { get; set; }
        public Customer? CustomerId_Navigation { get; set; }
    }
}
