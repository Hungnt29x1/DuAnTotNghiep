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
    public class SizeController : ControllerBase
    {
        private ISizeService iSizeService;
        public SizeController()
        {
            iSizeService = new SizeService();
        }

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all-Size")]
        public List<GProject.Data.DomainClass.Size> GetSize()
        {
            try
            {
                return iSizeService.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="Size"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-Size")]
        public bool AddSize([FromBody] GProject.Data.DomainClass.Size Size)
        {
            try
            {
                iSizeService.Create(Size);
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
        /// <param name="Size"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-Size")]
        public bool UpdateSize([FromBody] GProject.Data.DomainClass.Size Size)
        {
            try
            {
                iSizeService.Update(Size);
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
        /// <param name="Size"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-Size")]
        public bool DeleteSize(int id)
        {
            try
            {
                var Size = iSizeService.GetAll().FirstOrDefault(c => c.Id == id);
                iSizeService.Delete(Size);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
