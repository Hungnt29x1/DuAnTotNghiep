using GProject.Api.MyServices.IServices;
using GProject.Api.MyServices.Services;
using GProject.Data.DomainClass;
using GProject.WebApplication.Helpers;
using GProject.WebApplication.Models;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata;
using static IdentityServer4.Models.IdentityResources;

namespace GProject.WebApplication.Controllers
{
    [GProject.WebApplication.Services.Authorize]
    public class ProductMGRController : Controller
    {
        private IProductMGRService iProductService;
        public ProductMGRController()
        {
            iProductService = new ProductMGRService();
        }

        public async Task<ActionResult> Index(string sId, string sName,decimal? sImportPrice,int? sStatus, string? sBrand, string? category, decimal? sPrice, int pg = 1)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return RedirectToAction("AccessDenied", "Account");
                var lstBrand = await Commons.GetAll<Brand>(String.Concat(Commons.mylocalhost, "Brand/get-all-Brand"));
                var lstColor = await Commons.GetAll<ProductVariationDTO>(String.Concat(Commons.mylocalhost, "Color/get-all-Color"));
                var lstSize = await Commons.GetAll<ProductSizeVariation>(String.Concat(Commons.mylocalhost, "Size/get-all-Size"));
                var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
                //-- Lấy danh sách từ api
                var lstObjs = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));

                int valsStatus = sStatus.HasValue ? sStatus.Value : -1;

                if (!string.IsNullOrEmpty(sId))
                    lstObjs = lstObjs.Where(c => c.ProductCode.ToLower().Contains(sId.ToLower())).ToList();
                if (!string.IsNullOrEmpty(sName))
                    lstObjs = lstObjs.Where(c => c.Name.ToLower().Contains(sName.ToLower())).ToList();
                if (!string.IsNullOrEmpty(sBrand))
                    lstObjs = lstObjs.Where(c => c.BrandId == Convert.ToInt32(sBrand)).ToList();
                if (!string.IsNullOrEmpty(category))
                    lstObjs = lstObjs.Where(c => c.CategoryId == new Guid(category)).ToList();
                if (!string.IsNullOrEmpty(sImportPrice.ToString()))
                    lstObjs = (List<Product>)lstObjs.Where(c => c.ImportPrice >= sImportPrice);
                if (!string.IsNullOrEmpty(sPrice.ToString()))
                    lstObjs = (List<Product>)lstObjs.Where(c => c.Price <= sPrice);
                if (valsStatus != -1)
                    lstObjs = lstObjs.Where(c => c.Status == valsStatus).ToList();
                
                this.ViewData[nameof(sId)] = (object)sId;
                this.ViewData[nameof(sName)] = (object)sName;
                this.ViewData[nameof(sImportPrice)] = (object)sImportPrice;
                this.ViewData[nameof(sStatus)] = (object)valsStatus;
                this.ViewData[nameof(sBrand)] = (object)sBrand;
                this.ViewData[nameof(sPrice)] = (object)sPrice;

                const int pageSize = 10;
                if (pg < 1)
                    pg = 1;

                int recsCount = lstObjs.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = lstObjs.Skip(recSkip).Take(pageSize).ToList();
                this.ViewBag.Pager = pager;

                var result = new ProductMGRDTO() { ProductVariationList = lstProductvariation, ProductList = data, ProductVariationViewModel = lstColor.Where(c => c.Status == 1).ToList(), SizeList = lstSize };


                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
                    ViewData["Mess"] = HttpContext.Session.GetString("mess");
                HttpContext.Session.Remove("mess");
                return View(result);
            }
            catch (Exception)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                return RedirectToAction("AccessDenied", "Account");
            GProject.WebApplication.Services.ProductMGRService pService = new GProject.WebApplication.Services.ProductMGRService();
            ProductMGRDTO product = await pService.GetProductViewModel();
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductMGRDTO Product)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return RedirectToAction("AccessDenied", "Account");
                GProject.WebApplication.Services.ProductMGRService pService = new GProject.WebApplication.Services.ProductMGRService();
                bool result = await pService.Save(Product, User.Identity.Name != null ? User.Identity.Name : "");

                if (!result)
                    HttpContext.Session.SetString("mess", "Failed");
                else
                    HttpContext.Session.SetString("mess", "Success");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Update(Guid? id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                return RedirectToAction("AccessDenied", "Account");
            GProject.WebApplication.Services.ProductMGRService pService = new GProject.WebApplication.Services.ProductMGRService();
            ProductMGRDTO prd = new ProductMGRDTO();
            prd = await pService.GetProduct(id);
            return View(prd);
        }

        [HttpPost]
        public async Task<ActionResult> Update(ProductMGRDTO Product)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return RedirectToAction("AccessDenied", "Account");
                GProject.WebApplication.Services.ProductMGRService pService = new GProject.WebApplication.Services.ProductMGRService();
                bool result = await pService.Save(Product, User.Identity.Name != null ? User.Identity.Name : "");

                if (!result)
                    HttpContext.Session.SetString("mess", "Failed");
                else
                    HttpContext.Session.SetString("mess", "Success");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save(ProductMGRDTO Product)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return RedirectToAction("AccessDenied", "Account");
                GProject.WebApplication.Services.ProductMGRService pService = new GProject.WebApplication.Services.ProductMGRService();
                bool result = await pService.Save(Product, User.Identity.Name != null ? User.Identity.Name : "");
                
                if (!result)
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
        public async Task<JsonResult> ViewDetail(Guid? id)
        {
            GProject.WebApplication.Services.ProductMGRService pService = new GProject.WebApplication.Services.ProductMGRService();
            ProductMGRDTO prd = new ProductMGRDTO();
            prd = await pService.GetProduct(id);
            return Json(prd);
        }

        public async Task<ActionResult> ChangeStatus(Guid id)
        {
            GProject.WebApplication.Services.ProductMGRService pService = new GProject.WebApplication.Services.ProductMGRService();
            bool result = await pService.ChangeStatus(id);
            if (!result)
                HttpContext.Session.SetString("mess", "Failed");
            else
                HttpContext.Session.SetString("mess", "Success");
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> CheckName(string Name, Guid? Id)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return Json(new { success = false });

                var lstObjs = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
                var existName = lstObjs.Any(x => x.Name.ToLower() == Name.ToLower() && (!Id.HasValue || x.Id != Id.Value));
                return Json(new { success = !existName });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }
    }
}
