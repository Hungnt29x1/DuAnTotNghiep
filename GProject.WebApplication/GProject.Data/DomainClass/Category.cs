using GProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.DomainClass
{
    public class Category
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string SearchCount { get; set; }
        public int Status { get; set; }  // 0: sử dụng || 1: không sử dụng
        public string? Description { get; set; }
        public List<Product>? Products { get; set; }
    }
}
