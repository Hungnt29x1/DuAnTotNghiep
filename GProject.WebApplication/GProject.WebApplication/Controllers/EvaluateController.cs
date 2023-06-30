using GProject.Api.MyServices.IServices;
using GProject.Api.MyServices.Services;
using GProject.Data.DomainClass;
using GProject.WebApplication.Helpers;
using GProject.WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using static IdentityServer4.Models.IdentityResources;

namespace GProject.WebApplication.Controllers
{
    [GProject.WebApplication.Services.Authorize]
    public class EvaluateController : Controller
    {
        private IEvaluateService iEvaluateService;
        public EvaluateController()
        {
            iEvaluateService = new EvaluateService();
        }

        public async Task<ActionResult> Index(string sName, string sProdName, string fromDate, string toDate, int pg = 1)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return RedirectToAction("AccessDenied", "Account");
                ////-- Lấy danh sách từ api
                var lstCustomer = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
                var lstProducts = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
                var lstObjs = await Commons.GetAll<Evaluate>(String.Concat(Commons.mylocalhost, "Evaluate/get-all-Evaluate"));

                var data = lstCustomer
                            .Join(lstObjs, c => c.Id, e => e.CustomerId, (c, e) => new { Customer = c, Evaluate = e })
                            .Join(lstProducts, ce => ce.Evaluate.ProductId, p => p.Id, (ce, p) => new { Evaluate = ce.Evaluate, Customer = ce.Customer, Product = p })
                            .Select(x => new { Evaluate = x.Evaluate, Customer = x.Customer, Product = x.Product })
                            .OrderByDescending(c => c.Evaluate.CreateDate).ToList();
                ////-- truyền vào message nếu có thông báo
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
                    ViewData["Mess"] = HttpContext.Session.GetString("mess");
                HttpContext.Session.Remove("mess");

                List<EvaluateViewModel> lstData = Commons.ConverObject<List<EvaluateViewModel>>(data);
                if (!string.IsNullOrEmpty(sName))
                    lstData = lstData.Where(c => c.Customer.Name.ToLower().Contains(sName.ToLower())).ToList();
                if (!string.IsNullOrEmpty(sProdName))
                    lstData = lstData.Where(c => c.Product.Name.ToLower().Contains(sProdName.ToLower())).ToList();
                if (!string.IsNullOrEmpty(fromDate))
                    lstData = lstData.Where(c => c.Evaluate.CreateDate.Date >= DateTime.Parse(fromDate).Date).ToList();
                if (!string.IsNullOrEmpty(toDate))
                    lstData = lstData.Where(c => c.Evaluate.CreateDate.Date <= DateTime.Parse(toDate).Date).ToList();

                const int pageSize = 10;
                if (pg < 1)
                    pg = 1;
                var pager = new Pager(lstData.Count(), pg, pageSize);
                this.ViewBag.Pager = pager;
                this.ViewData[nameof(sName)] = (object)sName;
                this.ViewData[nameof(sProdName)] = (object)sProdName;
                this.ViewData[nameof(fromDate)] = (object)fromDate;
                this.ViewData[nameof(toDate)] = (object)toDate;
                return View(lstData.Skip((pg - 1) * pageSize).Take(pageSize).ToList());
            }
            catch (Exception)
            {

                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save(string Id, string ProductId, string Comment, int Rating)
        {
            try
            {
                var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
                if (customer == null) return RedirectToAction("Login", "Account");

                Guid id = string.IsNullOrEmpty(Id) ? Guid.NewGuid() : new Guid(Id);
                string url = Commons.mylocalhost;
                //-- Parse lại dữ liệu từ ViewModel
                var prd = new Evaluate() { Id = id, ProductId = new Guid(ProductId), CustomerId = customer.Id, Comment = Comment, Rating = Rating, CreateDate = DateTime.Now, UpdateDate = DateTime.Now };

                //-- Check hành động là Create hay update
                if (Id == null) url += "Evaluate/add-Evaluate";
                else url += "Evaluate/update-Evaluate";

                //-- Gửi request cho api sử lí
                bool result = await Commons.Add_or_UpdateAsync(prd, url);
                return RedirectToAction("ProductDetail", "Product", new { product_id = new Guid(ProductId) });
            }
            catch (Exception)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

        }


        public async Task<ActionResult> Delete(Guid? id)
        {
            try
            {
                var removeData = iEvaluateService.GetAll().FirstOrDefault(c => c.Id == id);
                if (!iEvaluateService.Delete(removeData))
                    HttpContext.Session.SetString("mess", "Failed");
                else
                    HttpContext.Session.SetString("mess", "Success");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

        }
    }
}
