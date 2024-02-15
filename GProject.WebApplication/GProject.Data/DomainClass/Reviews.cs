using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.DomainClass
{
    public class Reviews
    {
        public Guid? Id { get; set; }

        public Guid AccountID { get; set; }

        public Guid PostId { get; set; }

        public string Comment { get; set; }

        public DateTime? CreateDate { get; set; } = DateTime.Now;

        public Customer? CustomerId_Navigation { get; set; }
        public Posts? PostId_Navigation { get; set; }
    }
}
