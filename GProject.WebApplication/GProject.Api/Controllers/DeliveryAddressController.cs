using GProject.Api.MyServices.IServices;
using GProject.Api.MyServices.Services;
using GProject.Data.DomainClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryAddressController : ControllerBase
    {
        private readonly IDeliveryAddressService deliveryAddressService;
        public DeliveryAddressController()
        {
            deliveryAddressService = new DeliveryAddressService();
        }

        [HttpGet]
        [Route("get-all")]
        public List<DeliveryAddress> GetAll()
        {
            try
            {
                return deliveryAddressService.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("create")]
        public bool Create(DeliveryAddress obj)
        {
            try
            {
                if (obj == null) return false;
                deliveryAddressService.Create(obj);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("update")]
        public bool Update(DeliveryAddress obj)
        {
            try
            {
                if (obj == null) return false;
                deliveryAddressService.Update(obj);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("delete")]
        public bool Delete(int id)
        {
            try
            {
                deliveryAddressService.Delete(id);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
