using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GProject.WebApplication.Models
{
    public class ColorDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string HEXCode { get; set; }
        public int Status { get; set; }
        public IFormFile? Image_Upload { get; set; }
        public List<Color>? ColorList { get; set; }
    }
}
