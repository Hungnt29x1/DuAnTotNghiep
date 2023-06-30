using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using GProject.Data.MyRepositories.Repositories;

namespace GProject.Api.MyServices.Services
{
    public class DeliveryAddressService : IDeliveryAddressService
    {
        private readonly IDeliveryAddressRepository deliveryAddressRepository;
        public DeliveryAddressService() {
            deliveryAddressRepository = new DeliveryAddressRepository();
        }

        public bool Create(DeliveryAddress obj)
        {
            if(obj == null) return false;

            obj = new DeliveryAddress()
            {
                CustomerId = obj.CustomerId,
                Name = obj.Name,
                ProvinceID = obj.ProvinceID,
                ProvinceName = obj.ProvinceName,
                DistrictID = obj.DistrictID,
                DistrictName = obj.DistrictName,
                WardCode = obj.WardCode,
                WardName = obj.WardName,
                Address = obj.Address,
                PhoneNumber = obj.PhoneNumber,
                CreateDate = DateTime.Now
            };

            if(deliveryAddressRepository.Add(obj)) return true;

            return false;
        }

        public bool Delete(int id)
        {
            var result = deliveryAddressRepository.GetAll().FirstOrDefault(x => x.Id == id);
            if(result == null) return false;
            deliveryAddressRepository.Delete(result);
            return true;
        }

        public List<DeliveryAddress> GetAll()
        {
            return deliveryAddressRepository.GetAll().ToList();
        }

        public bool Update(DeliveryAddress obj)
        {
            var result = deliveryAddressRepository.GetAll().FirstOrDefault(x => x.Id == obj.Id);
            if(result == null) return false;
            result.ProvinceID = obj.ProvinceID;
            result.Name = obj.Name;
            result.ProvinceName= obj.ProvinceName;
            result.DistrictID= obj.DistrictID;
            result.DistrictName= obj.DistrictName;
            result.WardCode= obj.WardCode;
            result.WardName= obj.WardName;
            result.Address = obj.Address;
            result.PhoneNumber= obj.PhoneNumber;
            result.UpdateDate = DateTime.Now;
            deliveryAddressRepository.Update(result);
            return true;
        }
    }
}
