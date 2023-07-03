using GProject.Data.DomainClass;

namespace GProject.WebApplication.Models.DeliveryAddressAndShippingFee
{
    public class DeliveryAddressDto
    {
        public int Id { get; set; }
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string WardCode { get; set; }
        public string WardName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? CustomerId { get; set; }
        public List<DeliveryAddress> DeliveryAddressesList { get; set; }
    }
}