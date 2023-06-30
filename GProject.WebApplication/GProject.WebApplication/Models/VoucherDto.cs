using GProject.Data.DomainClass;

namespace GProject.WebApplication.Models
{
    public class VoucherDto
    {
        public int Id { get; set; }
        public string VoucherId { get; set; }
        public string Name { get; set; }
        public float DiscountRate { get; set; }  //mức giảm giá
        public string DiscountForm { get; set; } //hình thức
        public float? MaximumDiscount { get; set; } // giam toi da
        public int NumberOfVouchers { get; set; } //số lượng voucher
        public float MinimumOrder { get; set; }  //đơn hàng tối thiểu
        public DateTime ExpirationDate { get; set; } //ngày hết hạn
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? EmployeeId { get; set; }
        public int Status { get; set; }
        public List<Voucher> VoucherList { get; set; }
    }
}
