using GProject.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.DomainClass
{
    public class Deliver : IdentityRole<Guid>
    {
        public Guid? Id { get; set; }
        public string DeliverId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PersonalId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfJoin { get; set; }
        public string PhoneNumber { get; set; }
        public int Sex { get; set; } // 0:Nam || 1: Nữ
        public string Address { get; set; }
        public string LicensePlate { get; set; }
        public string BrandCar { get; set; }
        public string DrivingNumber { get; set; }
        public string DrivingLicense { get; set; }
        public int Status { get; set; } // 1: đang sẵn sàng nhận đơn || 0:Không sẵn sàng nhận đơn
        public string? Description { get; set; }
        public string Image { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
