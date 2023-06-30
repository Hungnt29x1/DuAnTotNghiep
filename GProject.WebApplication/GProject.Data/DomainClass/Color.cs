using GProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.DomainClass
{
    public class Color
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string HEXCode { get; set; }
        public int Status { get; set; } // 0: không sử dụng || 1: sử dụng
        public string Image { get; set; }
        public DateTime CreateOrUpdate { get; set; }
        public List<ProductVariation>? Variations { get; set; }
    }
}
