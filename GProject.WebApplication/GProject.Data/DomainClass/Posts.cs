using GProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.DomainClass
{
    public class Posts
    {
        public Guid? Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public PostType PostType { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string Image { get; set; }
        public int Status { get; set; } // 0: không sử dụng || 1: sử dụng
        public string? Description { get; set; }
    }
}
