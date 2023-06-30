using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GProject.Data.DomainClass;
using System.Collections.Generic;
using GProject.Api.MyServices.IServices;
using GProject.Api.MyServices.Services;
using System;
using System.Linq;
using System.Drawing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Claims;

namespace GProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ICustomerService customerService;
        private IEmployeeService employeeService;
        public UserController()
        {
            customerService = new CustomerService();
            employeeService = new EmployeeService();
        }

        [HttpPost]
        [Route("employee-login")]
        public async Task<Employee> EmployeeLogin(string email, string password)
        {
            try
            {
                var empData = employeeService.Login(email, password);
                return empData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("Customer-login")]
        public async Task<Customer> CustomerLogin(string email, string password)
        {
            try
            {
                var cusData = customerService.Login(email, password);
                return cusData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task Login()
        {
            try
            {
                //var empData = employeeService.Login(email, password);
                //var cusData = customerService.Login(email, password);

                ////-- Kiểm tra thông tin và phân quyền
                //if (empData != null)
                //{
                //    return (empData, null);
                //}
                //else if (cusData != null)
                //{
                //    return (null, cusData);
                //}
                //return (null, null);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
