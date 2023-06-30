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
    public class ProductMGRController : ControllerBase
    {
        private IProductMGRService iProductService;
        public ProductMGRController()
        {
            iProductService = new ProductMGRService();
        }

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all-Product-mgr")]
        public List<GProject.Data.DomainClass.Product> GetProduct()
        {
            try
            {
                return iProductService.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-Product-mgr")]
        public bool AddProduct([FromBody] GProject.Data.DomainClass.Product Product)
        {
            try
            {
                iProductService.Create(Product);
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
        /// <param name="Product"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-Product-mgr")]
        public bool UpdateProduct([FromBody] GProject.Data.DomainClass.Product Product)
        {
            try
            {
                iProductService.Update(Product);
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
        /// <param name="Product"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-Product-mgr")]
        public bool DeleteProduct(Guid id)
        {
            try
            {
                var Product = iProductService.GetAll().FirstOrDefault(c => c.Id == id);
                iProductService.Delete(Product);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
