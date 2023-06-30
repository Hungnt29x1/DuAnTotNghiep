using GProject.Data.DomainClass;
using GProject.WebApplication.Helpers;
using GProject.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Xml.Linq;

namespace GProject.WebApplication.Controllers
{
    public class VoucherController : Controller
    {
        public async Task<IActionResult> Index(string? ma, string? ten, int? trangthai, float? giamgia_tu, float? giamgia_den, string? hinhthuc, int pg = 1)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() != "manager")
                    return RedirectToAction("AccessDenied", "Account");
                //-- Lấy danh sách từ api
                var lstObjs = await Commons.GetAll<Voucher>(String.Concat(Commons.mylocalhost, "Voucher/get-all"));
                ViewData["Ten"] = ten;
                ViewData["Ma"] = ma;
                ViewData["trangthai"] = trangthai;
                ViewData["hinhthuc"] = hinhthuc;
                ViewData["giamgia_tu"] = giamgia_tu;
                ViewData["giamgia_den"] = giamgia_den;

                if (!string.IsNullOrEmpty(ten))
                {
                    lstObjs = lstObjs.Where(c => c.Name.ToLower().Contains(ten.ToLower())).ToList();                    
                }
                if (!string.IsNullOrEmpty(ma))
                {
                    lstObjs = lstObjs.Where(c => c.VoucherId.ToLower().Contains(ma.ToLower())).ToList();                    
                }
                if (trangthai.HasValue)
                {
                    lstObjs = lstObjs.Where(c => c.Status == trangthai).ToList();
                    
                }
                if (!string.IsNullOrEmpty(hinhthuc))
                {
                    lstObjs = lstObjs.Where(c => c.DiscountForm == hinhthuc).ToList();
                    
                }
                if (giamgia_tu.HasValue  && !string.IsNullOrEmpty(hinhthuc))
                {
                    lstObjs = lstObjs.Where(c => c.DiscountRate >= giamgia_tu  && c.DiscountForm == hinhthuc).ToList();
                    
                }
                if (giamgia_den.HasValue && !string.IsNullOrEmpty(hinhthuc))
                {
                    lstObjs = lstObjs.Where(c => c.DiscountRate <= giamgia_den && c.DiscountForm == hinhthuc).ToList();
                   
                }

                const int pageSize = 10;
                if (pg < 1)
                    pg = 1;
                var pager = new Pager(lstObjs.Count(), pg, pageSize);
                this.ViewBag.Pager = pager;
                ViewData["pg"] = pg;

                var lstData = lstObjs.Skip((pg - 1) * pageSize).Take(pageSize).ToList();


                var data = new VoucherDto() { VoucherList = lstObjs.OrderByDescending(x => x.ExpirationDate).ToList() };
                //-- truyền vào message nếu có thông báo
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
                    ViewData["Mess"] = HttpContext.Session.GetString("mess");
                HttpContext.Session.Remove("mess");
                return View(data);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public async Task<ActionResult> Save(VoucherDto obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() != "manager")
                    return RedirectToAction("AccessDenied", "Account");
                string url = Commons.mylocalhost;

                string randomVoucherId = RandomString().ToString();
                var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
                var lstObjs = await Commons.GetAll<Voucher>(String.Concat(Commons.mylocalhost, "Voucher/get-all"));
                var resultVoucher = lstObjs.Where(x => x.VoucherId == randomVoucherId);
                if (resultVoucher != null) RandomString();
                if(obj.Id == 0) obj.VoucherId = randomVoucherId;

                //-- Parse lại dữ liệu từ ViewModel
                var prd = new Voucher()
                {
                    Id = obj.Id,
                    VoucherId = obj.VoucherId,
                    Name = obj.Name,
                    DiscountRate = obj.DiscountRate,
                    DiscountForm= obj.DiscountForm,
                    MaximumDiscount = obj.MaximumDiscount,
                    NumberOfVouchers = obj.NumberOfVouchers,
                    MinimumOrder= obj.MinimumOrder,
                    ExpirationDate= obj.ExpirationDate,
                    CreateDate = obj.CreateDate,
                    UpdateDate = DateTime.Now,
                    EmployeeId = customer.Id,
                    Status = obj.Status
                };

                //-- Check hành động là Create hay update
                if (obj.Id == 0) url += "Voucher/create";
                else url += "Voucher/update";

                //-- Gửi request cho api sử lí
                bool result = await Commons.Add_or_UpdateAsync(prd, url);
                if (!result)
                    HttpContext.Session.SetString("mess", "Failed");
                else
                    HttpContext.Session.SetString("mess", "Success");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        public async Task<JsonResult> Detail(int id)
        {
            var lstObjs = await Commons.GetAll<Voucher>(String.Concat(Commons.mylocalhost, "Voucher/get-all"));
            var data2 = lstObjs.FirstOrDefault(c => c.Id == id);
            return Json(data2);
        }

        public string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 7)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpGet]
        public async Task<bool> UpdateSatus(int id)
        {
            var dataVoucher = new UpdateNumberVoucherDto() { Id = id };
            await Commons.Add_or_UpdateAsync(dataVoucher, String.Concat(Commons.mylocalhost, "Voucher/update-status"));

            return true;
        }
    }
}
