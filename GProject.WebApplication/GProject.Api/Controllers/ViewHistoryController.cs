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
    public class ViewHistoryController : ControllerBase
    {
        private IViewHistoryService iViewHistoryService;
        public ViewHistoryController()
        {
            iViewHistoryService = new ViewHistoryService();
        }

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all-ViewHistory")]
        public List<GProject.Data.DomainClass.ViewHistory> GetViewHistory()
        {
            try
            {
                return iViewHistoryService.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="ViewHistory"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-ViewHistory")]
        public bool AddViewHistory([FromBody] GProject.Data.DomainClass.ViewHistory ViewHistory)
        {
            try
            {
                iViewHistoryService.Create(ViewHistory);
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
        /// <param name="ViewHistory"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-ViewHistory")]
        public bool UpdateViewHistory([FromBody] GProject.Data.DomainClass.ViewHistory ViewHistory)
        {
            try
            {
                iViewHistoryService.Update(ViewHistory);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
