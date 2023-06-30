using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GProject.WebApplication.Models
{
    public class CategoryDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string SearchCount { get; set; }
        public int Status { get; set; }
        public string? Description { get; set; }
        public List<Category>? CategoryList { get; set; }
    }
}
