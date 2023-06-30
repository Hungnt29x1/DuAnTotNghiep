using GProject.Data.DomainClass;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Security.Claims;

namespace GProject.WebApplication.Models
{
    public class CustomerDTO: IdentityUser
    {
        public Guid? Id { get; set; }
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public int Sex { get; set; } // 0:Nam || 1: Nữ
        public string Address { get; set; }
        public int Status { get; set; } // 0: bình thường || 1: khách hàng cần chú ý
        public string? Description { get; set; }
        public string Image { get; set; }
        public IFormFile? Image_Upload { get; set; }
        public List<Customer>? CustomerList { get; set; }
    }
}
