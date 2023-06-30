using GProject.Data.DomainClass;

namespace GProject.Api.MyServices.IServices
{
    public interface IDeliveryAddressService
    {
        public bool Create(DeliveryAddress obj);

        public bool Update(DeliveryAddress obj);

        public bool Delete(int id);

        public List<DeliveryAddress> GetAll();
    }
}
