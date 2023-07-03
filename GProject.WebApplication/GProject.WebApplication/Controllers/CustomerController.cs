using GProject.Api.MyServices.IServices;
using GProject.Api.MyServices.Services;
using GProject.Data.DomainClass;
using GProject.WebApplication.Helpers;
using GProject.WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Reflection.Metadata;
using System.Xml.Linq;
using X.PagedList;

namespace GProject.WebApplication.Controllers
{
    [GProject.WebApplication.Services.Authorize]
    public class CustomerController : Controller
    {
        private ICustomerService iCustomerService;
        public CustomerController()
        {
            iCustomerService = new CustomerService();
        }

        public async Task<ActionResult> Index(string sId, string sName, string sEmail, string sPhone, int? sGender, int? sStatus, int? page)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return RedirectToAction("AccessDenied", "Account");
                int valsGender = sGender.HasValue ? sGender.Value : -1;
                int valsStatus = sStatus.HasValue ? sStatus.Value : -1;
                var lstObjs = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));

                if (!string.IsNullOrEmpty(sId))
                    lstObjs = lstObjs.Where(c => c.CustomerId.ToLower().Contains(sId.ToLower())).ToList();
                if (!string.IsNullOrEmpty(sName))
                    lstObjs = lstObjs.Where(c => c.Name.ToLower().Contains(sName.ToLower())).ToList();
                if (!string.IsNullOrEmpty(sEmail))
                    lstObjs = lstObjs.Where(c => c.Email.ToLower().Contains(sEmail.ToLower())).ToList();
                if (!string.IsNullOrEmpty(sPhone))
                    lstObjs = lstObjs.Where(c => c.PhoneNumber.ToLower().Contains(sPhone.ToLower())).ToList();
                if (valsGender != -1)
                    lstObjs = lstObjs.Where(c => c.Sex == valsGender).ToList();
                if (valsStatus != -1)
                    lstObjs = lstObjs.Where(c => c.Status == valsStatus).ToList();

                if (page == null) page = 1;
                var pageNumber = page ?? 1;
                var pageSize = 10;

                //this.ViewBag.Pager = pager;
                this.ViewData[nameof(sId)] = (object)sId;
                this.ViewData[nameof(sName)] = (object)sName;
                this.ViewData[nameof(sEmail)] = (object)sEmail;
                this.ViewData[nameof(sPhone)] = (object)sPhone;
                this.ViewData[nameof(sGender)] = (object)valsGender;
                this.ViewData[nameof(sStatus)] = (object)valsStatus;
                //-- truyền vào message nếu có thông báo
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
                    ViewData["Mess"] = HttpContext.Session.GetString("mess");
                HttpContext.Session.Remove("mess");
                return View(lstObjs.OrderByDescending(c=>c.CreateDate).ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CheckPhone(string PhoneNumber, Guid? Id)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return Json(new { success = false });
                var lstObjs = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
                var employees = await Commons.GetAll<Employee>(String.Concat(Commons.mylocalhost, "Employee/get-all-Employee"));

                var existNameCustomer = lstObjs.Any(x => x.PhoneNumber == PhoneNumber && (!Id.HasValue || x.Id != Id.Value));
                var existNameEmployee = employees.Any(x => x.PhoneNumber == PhoneNumber);

                if(!existNameCustomer && !existNameEmployee)
                    return Json(new { success = true });
                else
                    return Json(new { success = false });  
                    
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public async Task<ActionResult> CheckEmail(string Email, Guid? Id)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return Json(new { success = false });
                var lstObjs = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
                var employees = await Commons.GetAll<Employee>(String.Concat(Commons.mylocalhost, "Employee/get-all-Employee"));
                var existNameCustomer = lstObjs.Any(x => x.Email == Email && (!Id.HasValue || x.Id != Id.Value));
                var existNameEmployee = employees.Any(x => x.Email == Email);

                if(!existNameCustomer && !existNameEmployee)
                    return Json(new { success = true });
                else
                    return Json(new { success = false });              
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save( CustomerDTO Customer)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return RedirectToAction("AccessDenied", "Account");
                var lstObjs = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
                var checkId = lstObjs.FirstOrDefault(c => c.Id == Customer.Id);
                //-- Parse lại dữ liệu từ ViewModel
                var cusdata = new Customer();
                cusdata.Id = Customer.Id;
                cusdata.Name = Customer.Name;
                cusdata.Email = Customer.Email;
                cusdata.CustomerId = Commons.RandomString(10);

                if (!string.IsNullOrEmpty(Customer.Password)) cusdata.Password = Customer.Password;
                if (Customer.Id != null)
                {
                    if (!string.IsNullOrEmpty(Customer.Password))
                    {
                        cusdata.Password = Customer.Password;
                    }
                    else { cusdata.Password = checkId.Password; }
                }
                else
                {
                    cusdata.Password = Customer.Password;
                }
                cusdata.CreateDate = DateTime.Now;
                cusdata.UpdateDate = DateTime.Now;
                cusdata.DateOfBirth = Customer.DateOfBirth;
                cusdata.PhoneNumber = Customer.PhoneNumber;
                cusdata.Sex = Customer.Sex;
                cusdata.Address = Customer.Address;
                cusdata.Status = Customer.Status;
                if (!string.IsNullOrEmpty(Customer.Description)) cusdata.Description = Customer.Description;
                
                if (Customer.Id != null)
                {
                    if (Customer.Image_Upload == null)
                    {
                        cusdata.Image = checkId.Image;
                    }
                    else
                    {
                        string full_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", Customer.Image_Upload.FileName);
                        using (var file = new FileStream(full_path, FileMode.Create))
                        {
                            Customer.Image_Upload.CopyTo(file);
                        }
                        cusdata.Image = Customer.Image_Upload.FileName;
                    }
                }
                else
                {
                    if (Customer.Image_Upload != null)
                    {
                        string full_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", Customer.Image_Upload.FileName);
                        using (var file = new FileStream(full_path, FileMode.Create))
                        {
                            Customer.Image_Upload.CopyTo(file);
                        }
                        cusdata.Image = Customer.Image_Upload.FileName;
                    }
                }
                string url = Commons.mylocalhost;


                //-- Check hành động là Create hay update
                if (Customer.Id == null) url += "Customer/add-Customer";
                else url += "Customer/update-Customer";

                //-- Gửi request cho api sử lí
                bool result = await Commons.Add_or_UpdateAsync(cusdata, url);
               
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

        }

        public async Task<JsonResult> Detail(Guid id)
        {
            var lstObjs = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
            var data = lstObjs.FirstOrDefault(c => c.Id == id);
            return Json(data);
        }
    }
}
