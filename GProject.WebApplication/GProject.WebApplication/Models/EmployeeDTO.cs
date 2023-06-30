using GProject.Data.DomainClass;
using GProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GProject.WebApplication.Models
{
    public class EmployeeDTO
    {
        public Guid? Id { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PersonalId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoin { get; set; }
        public string PhoneNumber { get; set; }
        public int Sex { get; set; } // 0:Nam || 1: Nữ
        public string Address { get; set; }
        public int Status { get; set; } // 0: đang làm việc || 1:đã nghỉ việc
        public EmployeePosition Position { get; set; }
        public string? Description { get; set; }
        public string Image { get; set; }
        public IFormFile? Image_Upload { get; set; }
        public List<Employee>? EmployeeList { get; set; }
    }
}
