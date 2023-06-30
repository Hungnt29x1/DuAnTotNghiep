using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GProject.Data.DomainClass;
using System.Collections.Generic;
using GProject.Api.MyServices.IServices;
using GProject.Api.MyServices.Services;
using System;
using System.Linq;
using System.Drawing;

namespace GProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerService iCustomerService;
        public CustomerController()
        {
            iCustomerService = new CustomerService();
        }

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all-Customer")]
        public List<GProject.Data.DomainClass.Customer> GetCustomer()
        {
            try
            {
                return iCustomerService.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-Customer")]
        public bool AddCustomer([FromBody] GProject.Data.DomainClass.Customer Customer)
        {
            try
            {
                iCustomerService.Create(Customer);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-Customer")]
        public bool UpdateCustomer([FromBody] GProject.Data.DomainClass.Customer Customer)
        {
            try
            {
                iCustomerService.Update(Customer);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-Customer")]
        public bool DeleteCustomer(Guid id)
        {
            try
            {
                var Customer = iCustomerService.GetAll().FirstOrDefault(c => c.Id == id);
                iCustomerService.Delete(Customer);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
