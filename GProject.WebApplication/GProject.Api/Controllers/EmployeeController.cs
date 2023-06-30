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
    public class EmployeeController : ControllerBase
    {
        private IEmployeeService iEmployeeService;
        public EmployeeController()
        {
            iEmployeeService = new EmployeeService();
        }

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all-Employee")]
        public List<GProject.Data.DomainClass.Employee> GetEmployee()
        {
            try
            {
                return iEmployeeService.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="Employee"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-Employee")]
        public bool AddEmployee([FromBody] GProject.Data.DomainClass.Employee Employee)
        {
            try
            {
                iEmployeeService.Create(Employee);
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
        /// <param name="Employee"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-Employee")]
        public bool UpdateEmployee([FromBody] GProject.Data.DomainClass.Employee Employee)
        {
            try
            {
                iEmployeeService.Update(Employee);
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
        /// <param name="Employee"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-Employee")]
        public bool DeleteEmployee(Guid id)
        {
            try
            {
                var Employee = iEmployeeService.GetAll().FirstOrDefault(c => c.Id == id);
                iEmployeeService.Delete(Employee);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
