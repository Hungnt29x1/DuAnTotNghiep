using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GProject.WebApplication.Models
{
    public class EvaluateCommentDTO
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Comment { get; set; }
        public List<Evaluate> Evaluates { get; set; }
        public Guid? ProductId { get; set; }
        public int Rating { get; set; }
        public List<EvaluateViewModel> EvaluatesViewModel { get; set;}
    }

    public class EvaluateViewModel {
        public Evaluate Evaluate { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
