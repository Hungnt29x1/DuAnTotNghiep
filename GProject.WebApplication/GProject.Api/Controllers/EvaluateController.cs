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
    public class EvaluateController : ControllerBase
    {
        private IEvaluateService iEvaluateService;
        public EvaluateController()
        {
            iEvaluateService = new EvaluateService();
        }

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all-Evaluate")]
        public List<GProject.Data.DomainClass.Evaluate> GetEvaluate()
        {
            try
            {
                return iEvaluateService.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="Evaluate"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-Evaluate")]
        public bool AddEvaluate([FromBody] GProject.Data.DomainClass.Evaluate Evaluate)
        {
            try
            {
                iEvaluateService.Create(Evaluate);
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
        /// <param name="Evaluate"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-Evaluate")]
        public bool UpdateEvaluate([FromBody] GProject.Data.DomainClass.Evaluate Evaluate)
        {
            try
            {
                iEvaluateService.Update(Evaluate);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        ///// <summary>
        ///// Xóa
        ///// </summary>
        ///// <param name="Evaluate"></param>
        ///// <returns></returns>
        //[HttpDelete]
        //[Route("delete-Evaluate")]
        //public bool DeleteEvaluate(int id)
        //{
        //    try
        //    {
        //        var Evaluate = iEvaluateService.GetAll().FirstOrDefault(c => c.Id == id);
        //        iEvaluateService.Delete(Evaluate);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
    }
}
