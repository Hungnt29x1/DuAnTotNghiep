using GProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.DomainClass
{
    public class Contact
    {
        public Guid? Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? EmployeeId { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; } = 1; // 0: không sử dụng || 1: sử dụng
        public Employee? EmployeeId_navigation { get; set; }
        public Customer? CustomerId_navigation { get; set; }
    }
}
