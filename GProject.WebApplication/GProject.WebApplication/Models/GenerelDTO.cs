using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GProject.WebApplication.Models
{
    public class SexDTO
    {
        public int ID { get; set; }
        public string Sex { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class MyChartData
    {
        public DateTime Day { get; set; }
        public decimal TongThanhTien { get; set; }
        public decimal TongLoiNhuan { get; set; }
        public double TongChiPhi { get; set; }
        public int TongDonHang { get; set; }
    }

    public class EmployeeStaticalDTO
    {
        public string Employee_Id { get; set; }
        public string Employee_Name { get; set; }
        public int Sex { get; set; }
        public int Day { get; set; }
        public string Phone_Number { get; set; }
        public int Total_Quantity { get; set; }
        public int Total_number_Sales { get; set; }
        public decimal Total_Revenue { get; set; }
    }

    public class CategoryStaticalDTP
    {
        public string CategoryName { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class EmailMessage
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class ProductStaticalDTO
    {
        public string ProdName { get; set; }
        public int CreateDate { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string ProdType { get; set; }
        public int Total_Quantity { get; set; }
        public decimal Total_Revenue { get; set; }
    }
}
