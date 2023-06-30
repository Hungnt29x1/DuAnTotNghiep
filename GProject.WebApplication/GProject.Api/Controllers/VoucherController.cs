using GProject.Api.Models;
using GProject.Api.MyServices.IServices;
using GProject.Api.MyServices.Services;
using GProject.Data.DomainClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService voucherService;
        public VoucherController()
        {
            voucherService = new VoucherService();
        }

        [HttpGet]
        [Route("get-all")]
        public List<Voucher> GetAll()
        {
            return voucherService.GetAll();
        }

        [HttpPost]
        [Route("create")]
        public bool Create(Voucher voucher)
        {
            try
            {
                if (voucher == null) return false;
                voucherService.Create(voucher);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("update")]
        public bool Update(Voucher voucher)
        {
            try
            {
                if (voucher == null) return false;
                voucherService.Update(voucher);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete]
        [Route("delete")]
        public bool Delete(int id)
        {
            try
            {
                if (voucherService.Delete(id)) return true;
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("update-number")]
        public bool UpdateNumber(UpdateNumberVoucherModel obj)
        {
            try
            {
                if (obj == null) return false;
                voucherService.UpdateNumber(obj.Id);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        [Route("update-status")]
        public bool UpdateStatus(UpdateNumberVoucherModel input)
        {
            try
            {
                voucherService.UpdateStatus(input.Id);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
