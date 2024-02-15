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
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Scontents { get; set; }
        public string Contents { get; set; }
        public string Thumb { get; set; }
        public bool Published { get; set; }
        public string Alias { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Author { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? CategoryId { get; set; }
        public bool IsHot { get; set; }
        public bool IsNewfeed { get; set; }
        public int? Views { get; set; }

        public Category? CategoryId_Navigation { get; set; }
        public Employee? EmployeeId_Navigation { get; set; }
        public List<Reviews>? Reviews { get; set; }
    }
}
