using GProject.Api.MyServices.IServices;
using GProject.Api.MyServices.Services;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using GProject.WebApplication.Helpers;
using GProject.WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection.Metadata;
using X.PagedList;
using static IdentityServer4.Models.IdentityResources;

namespace GProject.WebApplication.Controllers
{
    [GProject.WebApplication.Services.Authorize]
    public class PromotionController : Controller
    {
        private IPromotionRepository _IPromotionRepository;
        private IPromotionDetailRepository _IPromotionDetailRepository;
        public PromotionController()
        {
            _IPromotionRepository = new PromotionRepository();
            _IPromotionDetailRepository = new PromotionDetailRepository();
        }

        public async Task<ActionResult> Index(string sId, string sName, int? sPercent, int? sStatus, string sfromDiscountRate, string stoDiscountRate, int? page)
        {
            try
            {
                //if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() != "manager")
                //    return RedirectToAction("AccessDenied", "Account");
                int valsPercent = sPercent.HasValue ? sPercent.Value : -1;
                int valsStatus = sStatus.HasValue ? sStatus.Value : -1;
                //-- Lấy danh sách từ api
                var lstObjs = _IPromotionRepository.GetAll();

                if (!string.IsNullOrEmpty(sId))
                    lstObjs = lstObjs.Where(c => c.PromotionId.ToLower().Contains(sId.ToLower())).ToList();
                if (!string.IsNullOrEmpty(sName))
                    lstObjs = lstObjs.Where(c => c.PromotionName.ToLower().Contains(sName.ToLower())).ToList();
                if (valsPercent != -1)
                    lstObjs = lstObjs.Where(c => c.DiscountPercent == valsPercent).ToList();
                if (valsStatus != -1)
                    lstObjs = lstObjs.Where(c => (int)c.Status == valsStatus).ToList();
                if (!string.IsNullOrEmpty(sfromDiscountRate))
                    lstObjs = lstObjs.Where(c => c.DiscountRate >= decimal.Parse(sfromDiscountRate)).ToList();
                if (!string.IsNullOrEmpty(stoDiscountRate))
                    lstObjs = lstObjs.Where(c => c.DiscountRate <= decimal.Parse(stoDiscountRate)).ToList();

                if (page == null) page = 1;
                var pageNumber = page ?? 1;
                var pageSize = 10;

                //-- truyền vào message nếu có thông báo
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
                    ViewData["Mess"] = HttpContext.Session.GetString("mess");
                HttpContext.Session.Remove("mess");

                this.ViewData[nameof(sId)] = (object)sId;
                this.ViewData[nameof(sName)] = (object)sName;
                this.ViewData[nameof(sPercent)] = (object)valsPercent;
                this.ViewData[nameof(sStatus)] = (object)valsStatus;
                this.ViewData[nameof(sfromDiscountRate)] = (object)sfromDiscountRate;
                this.ViewData[nameof(stoDiscountRate)] = (object)stoDiscountRate;
                return View(lstObjs.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() != "manager")
                return RedirectToAction("AccessDenied", "Account");
            var lstPromotion = _IPromotionRepository.GetAll();
            var lstProduct = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
            //-- Lấy danh sách sản phẩm chưa đc giảm giá
            var result = lstProduct.Where(a => !lstPromotion.Select(b => b.Id).Contains(a.Id)).ToList();
            ViewData["ProductList"] = result;
            return View(new PromotionDTO());
        }
        public async Task<ActionResult> ReloadTable(string? id, string? price, string? promotionid)
        {
            try
            {
                var lstPromotion1 = _IPromotionRepository.GetAll();
                var lstPromotionDEtail = _IPromotionDetailRepository.GetAll();
                var lstPromotion = (from x in lstPromotion1 join y in lstPromotionDEtail on x.Id equals y.PromotionId where x.Status == Data.Enums.PromotionStatus.Happenning select y).ToList();
                var lstProduct = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
                List<Product> tableData = new List<Product>();

                //-- Lấy danh sách sản phẩm chưa đc giảm giá
                if (string.IsNullOrEmpty(promotionid))
                {
                    tableData = lstProduct.Where(a => !lstPromotion.Select(b => b.ProductId).Contains(a.Id)).ToList();
                    if (!string.IsNullOrEmpty(id))
                    {
                        tableData = tableData.Where(c => c.CategoryId == new Guid(id)).ToList();
                    }
                    if (!string.IsNullOrEmpty(price))
                    {
                        tableData = tableData.Where(c => c.Price > decimal.Parse(price)).ToList();
                    }
                }
                else
                {
                    var lstPromotionDetail = lstPromotion.Where(c => c.PromotionId == new Guid(promotionid)).ToList();
                    tableData = lstProduct.Join(lstPromotionDetail, x => x.Id, y => y.ProductId, (x, y) => x).ToList();
                }
                return PartialView("_ProductTable", tableData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(PromotionDTO promotion)
        {
            try
            {
                //-- Promotion
                var employee = HttpContext.Session.GetObjectFromJson<Employee>("userLogin");
                Promotion promotionData = new Promotion();
                promotionData.Id = Guid.NewGuid();
                promotionData.PromotionId = Commons.RandomString(15);
                promotionData.PromotionName = promotion.PromotionName;
                promotionData.DiscountRate = promotion.DiscountRate;
                promotionData.DiscountPercent = promotion.DiscountPercent;
                promotionData.CreateDate = DateTime.Now;
                promotionData.UpdateDate = DateTime.Now;
                promotionData.StartDate = promotion.StartDate;
                promotionData.EndDate = promotion.EndDate;
                promotionData.CategoryApply = promotion.CategoryApply;
                promotionData.ProductApply = promotion.ProductApply;
                promotionData.CreateBy = employee.Email;
                promotionData.Status = promotion.Status;
                promotionData.Description = promotion.Description;

                if (promotion.Image_Upload != null)
                {
                    string full_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", promotion.Image_Upload.FileName);
                    using (var file = new FileStream(full_path, FileMode.Create))
                    {
                        promotion.Image_Upload.CopyTo(file);
                    }
                    promotionData.Image = promotion.Image_Upload.FileName;
                }
                if (!_IPromotionRepository.Add(promotionData))
                {
                    HttpContext.Session.SetString("mess", "Failed");
                    return RedirectToAction("Index");
                }

                //-- PromotionDetail
                List<PromotionDetail> promotionDetailList = JsonConvert.DeserializeObject<List<PromotionDetail>>(promotion.ProductApply);
                foreach (var item in promotionDetailList)
                {
                    var proDetail = new PromotionDetail();
                    proDetail.PromotionId = promotionData.Id;
                    proDetail.ProductId = item.ProductId;
                    proDetail.InitialPrice = item.InitialPrice;
                    proDetail.CurrentPrice = item.CurrentPrice;
                    proDetail.Status = 0;

                    if (!_IPromotionDetailRepository.Add(proDetail))
                    {
                        HttpContext.Session.SetString("mess", "Failed");
                        return RedirectToAction("Index");
                    }
                }
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
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() != "manager")
                    return RedirectToAction("AccessDenied", "Account");
                var Promotion = _IPromotionRepository.GetAll().Where(c => c.Id == id).FirstOrDefault();
                if (Promotion == null)
                {
                    return RedirectToAction("AccessDenied", "Account");
                }
                var lstPromotionDetail = _IPromotionDetailRepository.GetAll().Where(c => c.PromotionId == id).ToList();
                var lstProduct = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
                var result = (from x in lstProduct join y in lstPromotionDetail on x.Id equals y.ProductId select x).ToList();
                //-- Lấy danh sách sản phẩm chưa đc giảm giá
                ViewData["ProductList"] = result;

                PromotionDTO pro = new PromotionDTO();
                pro.Id = Promotion.Id;
                pro.PromotionId = Promotion.PromotionId;
                pro.PromotionName = Promotion.PromotionName;
                pro.DiscountPercent = Promotion.DiscountPercent;
                pro.DiscountRate = Promotion.DiscountRate;
                pro.CreateDate = Promotion.CreateDate;
                pro.UpdateDate = Promotion.UpdateDate;
                pro.StartDate = Promotion.StartDate;
                pro.EndDate = Promotion.EndDate;
                pro.Image = Promotion.Image;
                pro.CategoryApply = Promotion.CategoryApply;
                pro.ProductApply = Promotion.ProductApply;
                pro.Status = Promotion.Status;
                pro.Description = Promotion.Description;
                pro.PromotionDetails = lstPromotionDetail;
                return View(pro);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        public ActionResult Update(PromotionDTO promotion)
        {
            try
            {
                //-- Promotion
                var employee = HttpContext.Session.GetObjectFromJson<Employee>("userLogin");
                Promotion promotionData = new Promotion();
                promotionData.Id = promotion.Id;
                promotionData.PromotionId = Commons.RandomString(15);
                promotionData.PromotionName = promotion.PromotionName;
                promotionData.DiscountRate = promotion.DiscountRate;
                promotionData.DiscountPercent = promotion.DiscountPercent;
                promotionData.UpdateDate = DateTime.Now;
                promotionData.StartDate = promotion.StartDate;
                promotionData.EndDate = promotion.EndDate;
                promotionData.CategoryApply = promotion.CategoryApply;
                promotionData.ProductApply = promotion.ProductApply;
                promotionData.CreateBy = employee.Email;
                promotionData.Status = promotion.Status;
                promotionData.Description = promotion.Description;

                if (promotion.Image_Upload != null)
                {
                    string full_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", promotion.Image_Upload.FileName);
                    using (var file = new FileStream(full_path, FileMode.Create))
                    {
                        promotion.Image_Upload.CopyTo(file);
                    }
                    promotionData.Image = promotion.Image_Upload.FileName;
                }
                if (!_IPromotionRepository.Update(promotionData))
                {
                    HttpContext.Session.SetString("mess", "Failed");
                    return RedirectToAction("Index");
                }

                //-- Xóa bỏ tất cả promotionDetail trước đó
                List<PromotionDetail> promotionDetails = _IPromotionDetailRepository.GetAll().Where(c => c.PromotionId == promotion.Id).ToList();
                foreach (var item in promotionDetails)
                {
                    if (!_IPromotionDetailRepository.Delete(item))
                        return RedirectToAction("AccessDenied", "Account");
                }

                //-- Thêm danh sách Promotion mới
                List<PromotionDetail> promotionDetailList = JsonConvert.DeserializeObject<List<PromotionDetail>>(promotion.ProductApply);
                foreach (var item in promotionDetailList)
                {
                    var proDetail = new PromotionDetail();
                    proDetail.PromotionId = promotionData.Id;
                    proDetail.ProductId = item.ProductId;
                    proDetail.InitialPrice = item.InitialPrice;
                    proDetail.CurrentPrice = item.CurrentPrice;
                    proDetail.Status = 0;

                    if (!_IPromotionDetailRepository.Add(proDetail))
                    {
                        HttpContext.Session.SetString("mess", "Failed");
                        return RedirectToAction("Index");
                    }
                }
                HttpContext.Session.SetString("mess", "Success");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        public async Task<ActionResult> searchByProductName(string prodName, string? id, string? price, string? promotionid)
        {
            try
            {
                var lstPromotion1 = _IPromotionRepository.GetAll();
                var lstPromotionDEtail = _IPromotionDetailRepository.GetAll();
                var lstPromotion = (from x in lstPromotion1 join y in lstPromotionDEtail on x.Id equals y.PromotionId where x.Status == Data.Enums.PromotionStatus.Happenning select y).ToList();
                var lstProduct = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
                List<Product> tableData = new List<Product>();

                if (!string.IsNullOrEmpty(prodName))
                {
                    lstProduct = lstProduct.Where(c => c.Name.ToLower().Contains(prodName.ToLower())).ToList();
                }
                //-- Lấy danh sách sản phẩm chưa đc giảm giá
                if (string.IsNullOrEmpty(promotionid))
                {
                    tableData = lstProduct.Where(a => !lstPromotion.Select(b => b.ProductId).Contains(a.Id)).ToList();
                    if (!string.IsNullOrEmpty(id) && id != "-1")
                    {
                        tableData = tableData.Where(c => c.CategoryId == new Guid(id)).ToList();
                    }
                    if (!string.IsNullOrEmpty(price))
                    {
                        tableData = tableData.Where(c => c.Price > decimal.Parse(price)).ToList();
                    }
                }
                else
                {
                    var lstPromotionDetail = lstPromotion.Where(c => c.PromotionId == new Guid(promotionid)).ToList();
                    tableData = lstProduct.Join(lstPromotionDetail, x => x.Id, y => y.ProductId, (x, y) => x).ToList();
                }
                return PartialView("_ProductTable", tableData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
