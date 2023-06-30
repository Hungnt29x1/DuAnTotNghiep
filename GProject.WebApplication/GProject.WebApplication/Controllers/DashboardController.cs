using GProject.Api.MyServices.IServices;
using GProject.Api.MyServices.Services;
using GProject.Data.DomainClass;
using GProject.WebApplication.Helpers;
using GProject.WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace GProject.WebApplication.Controllers
{
    [GProject.WebApplication.Services.Authorize]
    public class DashboardController : Controller
    {
        private IColorService iColorService;
        public DashboardController()
        {
            iColorService = new ColorService();
        }

        public async Task<ActionResult> Index(string fromDate, string toDate)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return RedirectToAction("AccessDenied", "Account");
                var lstOrders = await Commons.GetAll<Order>(String.Concat(Commons.mylocalhost, "Order/get-all-Order"));
                var lstOrderDetail = await Commons.GetAll<OrderDetail>(String.Concat(Commons.mylocalhost, "Order/get-all-Order-detail"));
                var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
                var lstProducts = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
                var lstEvaluate = await Commons.GetAll<Evaluate>(String.Concat(Commons.mylocalhost, "Evaluate/get-all-Evaluate"));
                var lstCustomer = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
                var lstEmployee = await Commons.GetAll<Employee>(String.Concat(Commons.mylocalhost, "Employee/get-all-Employee"));
                var lstCategories = await Commons.GetAll<Category>(String.Concat(Commons.mylocalhost, "Category/get-all-Category"));
                var lstBrands = await Commons.GetAll<Brand>(String.Concat(Commons.mylocalhost, "Brand/get-all-Brand"));

                //--Thông kê lợi nhuận = Tổng doanh thu - tổng chi phí
                var results = lstOrders
                                .Join(lstOrderDetail, b => b.Id, bhct => bhct.OrderId, (b, bhct) => new { Order = b, OrderDetail = bhct })
                                .Join(lstProductvariation, ab => ab.OrderDetail.ProductVariationId, s => s.Id, (ab, s) => new { ab.Order, ab.OrderDetail, Productvariation = s })
                                .Join(lstProducts, abs => abs.Productvariation.ProductId, p => p.Id, (abs, p) => new { abs.Order, abs.OrderDetail, abs.Productvariation, Product = p })
                                .Where(absp => absp.Order.PaymentDate != null && absp.Productvariation.Id == absp.OrderDetail.ProductVariationId)
                                .Where(absp => absp.Order.Status == Data.Enums.OrderStatus.Accomplished)
                                .OrderBy(absp => absp.Order.CreateDate)
                                .GroupBy(absp => new { absp.Order.CreateDate.Date })
                                .Select(g => new
                                {
                                    Day = g.Key.Date,
                                    TongDonHang = g.Count(),
                                    TongThanhTien = g.Sum(c => c.OrderDetail.Quantity * c.OrderDetail.Price),
                                    TongChiPhi = Convert.ToDouble(g.Sum(c => c.OrderDetail.Quantity * getTotalCost(c.Productvariation.Id).Result)),
                                    TongLoiNhuan = g.Sum(c => c.OrderDetail.Quantity * c.OrderDetail.Price) - g.Sum(c => c.OrderDetail.Quantity * getTotalCost(c.Productvariation.Id).Result)
                                })
                                .ToList();

                //-- Thống kê theo danh mục sản phẩm
                var lstOrderForcategoryStatical = lstOrders;
                if (!string.IsNullOrEmpty(fromDate))
                {
					lstOrderForcategoryStatical = lstOrderForcategoryStatical.Where(c => c.PaymentDate != null && c.PaymentDate >= DateTime.Parse(fromDate) && c.Status == Data.Enums.OrderStatus.Accomplished).ToList();
				}
				if (!string.IsNullOrEmpty(toDate))
				{
					lstOrderForcategoryStatical = lstOrderForcategoryStatical.Where(c => c.PaymentDate != null && c.PaymentDate <= DateTime.Parse(toDate) && c.Status == Data.Enums.OrderStatus.Accomplished).ToList();
				}
				//-- Thống kê theo danh mục sản phẩm
				var resultsCategoryStatical = lstOrderForcategoryStatical
							.Join(lstOrderDetail, b => b.Id, bhct => bhct.OrderId, (b, bhct) => new { Order = b, OrderDetail = bhct })
                            .Join(lstProductvariation, ab => ab.OrderDetail.ProductVariationId, s => s.Id, (ab, s) => new { ab.Order, ab.OrderDetail, Productvariation = s })
                            .Join(lstProducts, abs => abs.Productvariation.ProductId, p => p.Id, (abs, p) => new { abs.Order, abs.OrderDetail, abs.Productvariation, Product = p })
                            .Where(absp => absp.Order.PaymentDate != null && absp.Productvariation.Id == absp.OrderDetail.ProductVariationId)
                            .Where(absp => absp.Order.Status == Data.Enums.OrderStatus.Accomplished)
                            .OrderBy(absp => absp.Order.PaymentDate)
                            .GroupBy(absp => new { absp.Product.CategoryId })
                            .Select(g => new
                            {
                                CategoryName = lstCategories.FirstOrDefault(x => x.Id == g.Key.CategoryId)?.Name ?? "",
                                TotalRevenue = g.Sum(c => c.OrderDetail.Quantity * c.OrderDetail.Price)
                            })
                            .ToList();

                //--Top 5 sản phẩm bán chạy
                var top5Product = lstOrders
                                .Join(lstOrderDetail, a => a.Id, b => b.OrderId, (a, b) => new { Order = a, OrderDetail = b })
                                .Join(lstProductvariation, ab => ab.OrderDetail.ProductVariationId, c => c.Id, (ab, c) => new { Order = ab.Order, OrderDetail = ab.OrderDetail, ProductVariation = c })
                                .Join(lstProducts, abc => abc.ProductVariation.ProductId, d => d.Id, (abc, d) => new { abc.Order, abc.OrderDetail, abc.ProductVariation, Product = d })
                                .Join(lstBrands, abcd => abcd.Product.BrandId, e => e.Id, (abcd, e) => new { abcd.Order, abcd.OrderDetail, abcd.ProductVariation, abcd.Product, Brand = e })
                                .Join(lstCategories, abcde => abcde.Product.CategoryId, p => p.Id, (abcde, p) => new { abcde.Order, abcde.OrderDetail, abcde.ProductVariation, abcde.Product, abcde.Brand, Category = p })
                                .Where(absp => absp.Order.Status == Data.Enums.OrderStatus.Accomplished)
                                .GroupBy(abcde => new
                                {
                                    Month = abcde.Order.CreateDate.Month,
                                    ProdName = abcde.Product.Name,
                                    Category = abcde.Category.Name,
                                    Brand = abcde.Brand.Name,
                                    ProdType = abcde.Product.ProductType
                                })
                                .OrderByDescending(g => g.Key.Month)
                                .Take(5)
                                .Select(g => new
                                {
                                    ProdName = g.Key.ProdName,
                                    CreateDate = g.Key.Month,
                                    Category = g.Key.Category,
                                    Brand = g.Key.Brand,
                                    ProdType = g.Key.ProdType,
                                    Total_Quantity = g.Sum(c => c.OrderDetail.Quantity),
                                    Total_Revenue = g.Sum(c => c.OrderDetail.Quantity * c.OrderDetail.Price),
                                }).OrderByDescending(g => g.Total_Revenue)
                                .ToList();

                //-- Số đơn hàng
                var OrderDataStatical = lstOrders.Join(lstOrderDetail, x => x.Id, y => y.OrderId, (x, y) => new { Order = x, OrderDetail = y }).ToList();

                //-- Lọc
                if (!string.IsNullOrEmpty(fromDate))
                {
                    OrderDataStatical = OrderDataStatical.Where(c => c.Order.CreateDate >= DateTime.Parse(fromDate)).ToList();
                    results = results.Where(c => c.Day >= DateTime.Parse(fromDate)).ToList();
                    lstEvaluate = lstEvaluate.Where(c => c.CreateDate >= DateTime.Parse(fromDate)).ToList();
                    lstProducts = lstProducts.Where(c => c.CreateDate >= DateTime.Parse(fromDate)).ToList();
                    lstCustomer = lstCustomer.Where(c => c.CreateDate >= DateTime.Parse(fromDate)).ToList();
                    top5Product = top5Product.Where(c => c.CreateDate >= DateTime.Parse(fromDate).Month).ToList();
                }
                if (!string.IsNullOrEmpty(toDate))
                {
                    OrderDataStatical = OrderDataStatical.Where(c => c.Order.CreateDate <= DateTime.Parse(toDate)).ToList();
                    results = results.Where(c => c.Day <= DateTime.Parse(toDate)).ToList();
                    lstEvaluate = lstEvaluate.Where(c => c.CreateDate <= DateTime.Parse(toDate)).ToList();
                    lstProducts = lstProducts.Where(c => c.CreateDate <= DateTime.Parse(toDate)).ToList();
                    lstCustomer = lstCustomer.Where(c => c.CreateDate <= DateTime.Parse(toDate)).ToList();
                    top5Product = top5Product.Where(c => c.CreateDate <= DateTime.Parse(toDate).Month).ToList();
                }
                if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
                {
                    top5Product = top5Product.Where(c => c.CreateDate == DateTime.Now.Month).ToList();
                }
                ViewBag.CountOrderComplete = OrderDataStatical.Select(c => c.Order).Where(c => c.Status == Data.Enums.OrderStatus.Accomplished).ToList().Count();
                ViewBag.CountOrderDelivery = OrderDataStatical.Select(c => c.Order).Where(c => c.Status == Data.Enums.OrderStatus.Delivery).ToList().Count();
                ViewBag.CountOrderInProgress = OrderDataStatical.Select(c => c.Order).Where(c => c.Status == Data.Enums.OrderStatus.InProgress).ToList().Count();
                ViewBag.CountOrderCanceled = OrderDataStatical.Select(c => c.Order).Where(c => c.Status == Data.Enums.OrderStatus.Canceled).ToList().Count();
                ViewBag.CountProduct = OrderDataStatical.Sum(c => c.OrderDetail.Quantity);
                ViewBag.CountEvaluate = lstEvaluate.Count();
                ViewBag.CountLike = lstProducts.Sum(c => c.LikeCount);
                ViewBag.CountView = lstProducts.Sum(c => c.ViewCount);
                ViewBag.CountCustomer = lstCustomer.Count();

                this.ViewData[nameof(fromDate)] = (object)fromDate;
                this.ViewData[nameof(toDate)] = (object)toDate;

                this.ViewData["ProductStatical"] = Commons.ConverObject<List<ProductStaticalDTO>>(top5Product);
                this.ViewData["CategoryStatical"] = Commons.ConverObject<List<CategoryStaticalDTP>>(resultsCategoryStatical);
                return View(Commons.ConverObject<List<MyChartData>>(results));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        public async Task<decimal> getTotalCost(Guid? productVariatinId)
        {
            var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
            var lstProducts = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));

            return lstProducts.Join(lstProductvariation, x => x.Id, y => y.ProductId, (x, y) => new { Product = x, Variation = y })
                           .Where(xy => xy.Variation.Id == productVariatinId)
                           .Select(xy => xy.Product.ImportPrice)
                           .FirstOrDefault().NullToDecimal();
        }
    }
}
