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
    public class ProductVariationController : ControllerBase
    {
        private IProductVariationService iProductVariationService;
        public ProductVariationController()
        {
            iProductVariationService = new ProductVariationService();
        }

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all-ProductVariation")]
        public List<GProject.Data.DomainClass.ProductVariation> GetProductVariation()
        {
            try
            {
                return iProductVariationService.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="ProductVariation"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-ProductVariation")]
        public bool AddProductVariation([FromBody] GProject.Data.DomainClass.ProductVariation ProductVariation)
        {
            try
            {
                iProductVariationService.Create(ProductVariation);
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
        /// <param name="ProductVariation"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-ProductVariation")]
        public bool UpdateProductVariation([FromBody] GProject.Data.DomainClass.ProductVariation ProductVariation)
        {
            try
            {
                iProductVariationService.Update(ProductVariation);
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
        /// <param name="ProductVariation"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-ProductVariation")]
        public bool DeleteProductVariation(Guid id)
        {
            try
            {
                var ProductVariation = iProductVariationService.GetAll().FirstOrDefault(c => c.Id == id);
                iProductVariationService.Delete(ProductVariation);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
