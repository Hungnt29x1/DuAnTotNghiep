using GProject.Data.DomainClass;
using GProject.WebApplication.Helpers;
using GProject.WebApplication.Models;
using GProject.WebApplication.Models.DeliveryAddressAndShippingFee;
using GProject.WebApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace GProject.WebApplication.Controllers
{
    public class DeliveryAddressController : Controller
    {
        private readonly DeliveryAddressAndShippingFeeService _deliveryAddressAndShippingFeeService;
        public DeliveryAddressController()
        {
            _deliveryAddressAndShippingFeeService = new DeliveryAddressAndShippingFeeService();
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] DeliveryAddressDto obj)
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
            var lstObjs = await Commons.GetAll<DeliveryAddress>(String.Concat(Commons.mylocalhost, "DeliveryAddress/get-all"));

            var checkExistence = lstObjs.FirstOrDefault(x =>
                x.ProvinceName == obj.ProvinceName &&
                x.DistrictName == obj.DistrictName &&
                x.WardName == obj.WardName &&
                x.Name == obj.Name &&
                x.PhoneNumber == obj.PhoneNumber &&
                x.Address == obj.Address &&
                x.CustomerId == customer.Id
                );

            if ((checkExistence != null && checkExistence.Id != obj.Id)) return false;

            string url = Commons.mylocalhost;

            //-- Parse lại dữ liệu từ ViewModel
            var prd = new DeliveryAddress()
            {
                Id = obj.Id,
                ProvinceID = obj.ProvinceID,
                ProvinceName = obj.ProvinceName,
                DistrictID = obj.DistrictID,
                DistrictName = obj.DistrictName,
                WardCode = obj.WardCode,
                WardName = obj.WardName,
                Name = obj.Name,
                PhoneNumber = obj.PhoneNumber,
                Address = obj.Address,
                CustomerId = customer.Id
            };

            //-- Check hành động là Create hay update
            if (obj.Id == 0) url += "DeliveryAddress/create";
            else url += "DeliveryAddress/update";

            //-- Gửi request cho api sử lí
            bool result = await Commons.Add_or_UpdateAsync(prd, url);
            if (!result)
                HttpContext.Session.SetString("mess", "Failed");
            else
                HttpContext.Session.SetString("mess", "Success");

            return true;
        }

        [HttpGet]
        public async Task<bool> Delete(int id)
        {
            await Commons.GetAll<DeliveryAddress>(String.Concat(Commons.mylocalhost, "DeliveryAddress/delete?id=" + id));
            return true;
        }

        public async Task<JsonResult> Detail(int id)
        {
            var lstObjs = await Commons.GetAll<DeliveryAddress>(String.Concat(Commons.mylocalhost, "DeliveryAddress/get-all"));
            var data = lstObjs.FirstOrDefault(c => c.Id == id);
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetProvinces()
        {
            var districts = await _deliveryAddressAndShippingFeeService.GetDataProvincesAsync();
            return Json(districts);
        }

        [HttpGet]
        public async Task<JsonResult> GetDistricts(int id)
        {
            var districts = await _deliveryAddressAndShippingFeeService.GetDataDistrictsAsync(id);
            return Json(districts);
        }

        [HttpGet]
        public async Task<JsonResult> GetWards(int id)
        {
            var wards = await _deliveryAddressAndShippingFeeService.GetDataWardAsync(id);
            return Json(wards);
        }

        [HttpGet]
        public async Task<JsonResult> ShippingFee(int district_id, string ward_code)
        {
            var fee = await _deliveryAddressAndShippingFeeService.ShippingFee(district_id, ward_code);
            return Json(fee);
        }

        [HttpGet]
        public async Task<JsonResult> GetDataDeliveryAddress()
        {
            try
            {
                Customer cus = new Customer();
                var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
                var lstObjs = await Commons.GetAll<DeliveryAddress>(String.Concat(Commons.mylocalhost, "DeliveryAddress/get-all"));

                return Json(lstObjs.Where(x => x.CustomerId == customer.Id));
            }
            catch (Exception)
            {
                return Json(null);
            }
        }
    }
}