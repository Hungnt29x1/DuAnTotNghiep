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
    public class Customer : IdentityUser
    {
        public Guid? Id { get; set; }
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public int Sex { get; set; } // 0:Nam || 1: Nữ
        public string Address { get; set; }
        public int Status { get; set; }  // 0: bình thường || 1: khách hàng cần chú ý
        public string? Description { get; set; }
        public string Image { get; set; }
        public string? GoogleId { get; set; }
        public List<Cart>? Carts { get; set; }
        public List<Order>? Orders { get; set; }
        public List<Contact>? Contacts { get; set; }
        public List<ViewHistory>? ViewHistories { get; set; }
        public List<FavoriteProduct>? FavoriteProducts { get; set; }
        public List<Evaluate>? Evaluates { get; set; }
    }
}
