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
    public class BrandController : ControllerBase
    {
        private IBrandService iBrandService;
        public BrandController()
        {
            iBrandService = new BrandService();
        }

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all-Brand")]
        public List<GProject.Data.DomainClass.Brand> GetBrand()
        {
            try
            {
                return iBrandService.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="Brand"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-Brand")]
        public bool AddBrand([FromBody] GProject.Data.DomainClass.Brand Brand)
        {
            try
            {
                iBrandService.Create(Brand);
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
        /// <param name="Brand"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-Brand")]
        public bool UpdateBrand([FromBody] GProject.Data.DomainClass.Brand Brand)
        {
            try
            {
                iBrandService.Update(Brand);
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
        /// <param name="Brand"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-Brand")]
        public bool DeleteBrand(int id)
        {
            try
            {
                var Brand = iBrandService.GetAll().FirstOrDefault(c => c.Id == id);
                iBrandService.Delete(Brand);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
