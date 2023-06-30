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
    public class ColorController : ControllerBase
    {
        private IColorService iColorService;
        public ColorController()
        {
            iColorService = new ColorService();
        }

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all-Color")]
        public List<GProject.Data.DomainClass.Color> GetColor()
        {
            try
            {
                return iColorService.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="Color"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-Color")]
        public bool AddColor([FromBody] GProject.Data.DomainClass.Color Color)
        {
            try
            {
                iColorService.Create(Color);
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
        /// <param name="Color"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-Color")]
        public bool UpdateColor([FromBody] GProject.Data.DomainClass.Color Color)
        {
            try
            {
                iColorService.Update(Color);
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
        /// <param name="Color"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-Color")]
        public bool DeleteColor(int id)
        {
            try
            {
                var color = iColorService.GetAll().FirstOrDefault(c => c.Id == id);
                iColorService.Delete(color);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
