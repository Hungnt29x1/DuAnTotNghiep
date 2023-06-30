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
    public class CategoryController : ControllerBase
    {
        private ICategoryService iCategoryService;
        public CategoryController()
        {
            iCategoryService = new CategoryService();
        }

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all-Category")]
        public List<GProject.Data.DomainClass.Category> GetCategory()
        {
            try
            {
                return iCategoryService.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-Category")]
        public bool AddCategory([FromBody] GProject.Data.DomainClass.Category Category)
        {
            try
            {
                iCategoryService.Create(Category);
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
        /// <param name="Category"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-Category")]
        public bool UpdateCategory([FromBody] GProject.Data.DomainClass.Category Category)
        {
            try
            {
                iCategoryService.Update(Category);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
