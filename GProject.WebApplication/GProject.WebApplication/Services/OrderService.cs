using GProject.Data.DomainClass;
using GProject.WebApplication.Helpers;
using GProject.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Xml.Linq;

namespace GProject.WebApplication.Services
{
    public class OrderService
    {
        public async Task<bool> AddToOrder(int selectVoucher, string cGiamGia, string cShippingFee, string cTotalMoney, string ShippingFullName, string ShippingPhone,
            string ShippingCity, string ShippingDistrict, string ShippingTown, string ShippingAddress, string ShippingEmail, string cDescription, int PaymentType = 0, Guid? customer_id = null, List<ProdOrder>? prodOrders = null)
        {
            var lstCart = await Commons.GetAll<Cart>(String.Concat(Commons.mylocalhost, "Cart/get-all-Cart"));
            var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
            var lstCartDetail = await Commons.GetAll<CartDetail>(String.Concat(Commons.mylocalhost, "Cart/get-all-cart-detail"));

            string strUrl = "";
            Guid order_id = Guid.NewGuid();


            //Order
            Order order = new Order();
            order.OrderId = string.Concat(DateTime.Now.ToString("ddMMyy"), Commons.RandomString(9));
            order.Id = order_id;
            order.CustomerId = customer_id;
            order.CreateDate = DateTime.Now;
            order.UpdateDate = DateTime.Now;
            order.PaymentType = (GProject.Data.Enums.PaymentType)PaymentType;
            order.ShippingFullName = ShippingFullName;
            order.ShippingCountry = "Việt Nam";
            order.ShippingCity = ShippingCity.NullToString();
            order.ShippingDistrict = ShippingDistrict.NullToString();
            order.ShippingTown = ShippingTown.NullToString();
            order.ShippingAddress = ShippingAddress.NullToString();
            order.ShippingPhone = ShippingPhone;
            order.ShippingEmail = ShippingEmail;
            order.TotalMoney = decimal.Parse(cTotalMoney);
            //order.ShippingFee = decimal.Parse(cShippingFee);
            order.ShippingFee = cShippingFee != null ? decimal.Parse(cShippingFee) : (decimal?)null;
            order.Description = cDescription;
            order.Status = GProject.Data.Enums.OrderStatus.InProgress;
            order.VoucherId = selectVoucher;
            order.DiscountRate = Convert.ToSingle(cGiamGia);
            if (PaymentType == 1)
            {
                order.PaymentDate = DateTime.Now;
            }
            strUrl = String.Concat(Commons.mylocalhost, "Order/add-Order");
            if (!await Commons.Add_or_UpdateAsync(order, strUrl))
                return false;




            //-- Add vào bảng OrderDetail và xóa nó khỏi giỏ hàng
            foreach (var item in prodOrders)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = order_id;
                orderDetail.ProductVariationId = new Guid(item.prodVariationId);
                orderDetail.Price = decimal.Parse(item.price.ToString());
                orderDetail.Quantity = item.quantity;
                orderDetail.TotalMoney = decimal.Parse(item.totalMoneyItem.ToString());
                orderDetail.Status = GProject.Data.Enums.OrderDetailStatus.Watting;

                //-- Add vào OrderDetail
                if (!await Commons.Add_or_UpdateAsync(orderDetail, String.Concat(Commons.mylocalhost, "Order/add-Order-detail")))
                    return false;


                var result = lstCartDetail.FirstOrDefault(x => x.CartId.ToString() == item.cartId && x.ProductVariationId.ToString() == item.prodVariationId);

                if (result != null)
                {
                    //-- Add thành công vào OrderDetail -> Remove khỏi CartDetail
                    string urlRemoveCartDetail = string.Concat(Commons.mylocalhost, "Cart/delete-cart-detail?id=", new Guid(item.cartId), "&productVariation_id=", new Guid(item.prodVariationId));
                    var RestRemoveCartDetail = new RestSharpHelper(urlRemoveCartDetail);
                    var resRemoveCartDetail = await RestRemoveCartDetail.RequestBaseAsync(urlRemoveCartDetail, RestSharp.Method.Delete);
                    if (!resRemoveCartDetail.IsSuccessful)
                        return false;
                }
                else
                {
                    //-- Giảm số lượng còn lại của ProductVariation
                    ProductVariation productVariation = lstProductvariation.FirstOrDefault(c => c.Id == new Guid(item.prodVariationId));
                    if (productVariation == null) return false;
                    productVariation.QuantityInStock = productVariation.QuantityInStock - item.quantity;
                    if (!await Commons.Add_or_UpdateAsync(productVariation, String.Concat(Commons.mylocalhost, "ProductVariation/update-ProductVariation")))
                        return false;
                }
            }

            var isExist = (from c in lstCart join b in lstCartDetail on c.Id equals b.CartId where c.CustomerId == customer_id select b).FirstOrDefault();
            if (isExist == null)
            {
                string urlRemoveCart = string.Concat(Commons.mylocalhost, "Cart/delete-Cart?id=", new Guid(prodOrders.Select(c => c.cartId).FirstOrDefault().NullToString()));
                var RestRemoveCart = new RestSharpHelper(urlRemoveCart);
                var resultRemoveCart = await RestRemoveCart.RequestBaseAsync(urlRemoveCart, RestSharp.Method.Delete);
                if (!resultRemoveCart.IsSuccessful)
                    return false;
            }

            return true;
        }


        public async Task<bool> BuyNow(int selectVoucher, string cGiamGia, string cShippingFee, string pTotalMoney, string ShippingFullName, string ShippingPhone,
            string ShippingCity, string ShippingDistrict, string ShippingTown, string ShippingAddress, string ShippingEmail, string cDescription, int PaymentType = 0, Guid? customer_id = null, List<ProdOrder>? prodOrders = null)
        {
            var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
            string strUrl = "";
            Guid order_id = Guid.NewGuid();

            DateTime? order_date = null;
            if (PaymentType == 1)
            {
                order_date = DateTime.Now;
            }

            //Order
            Order order = new Order();
            order.OrderId = string.Concat(DateTime.Now.ToString("ddMMyy"), Commons.RandomString(9));
            order.Id = order_id;
            order.CustomerId = customer_id;
            order.CreateDate = DateTime.Now;
            order.UpdateDate = DateTime.Now;
            order.PaymentType = (GProject.Data.Enums.PaymentType)PaymentType;
            order.ShippingFullName = ShippingFullName;
            order.ShippingCountry = "Việt Nam";
            order.ShippingCity = ShippingCity.NullToString();
            order.ShippingDistrict = ShippingDistrict.NullToString();
            order.ShippingTown = ShippingTown.NullToString();
            order.ShippingAddress = ShippingAddress.NullToString();
            order.ShippingPhone = ShippingPhone;
            order.ShippingEmail = ShippingEmail;
            order.TotalMoney = decimal.Parse(pTotalMoney);
            //order.ShippingFee = decimal.Parse(cShippingFee);
            order.ShippingFee = cShippingFee != null ? decimal.Parse(cShippingFee) : (decimal?)null;
            order.Description = cDescription;
            order.Status = GProject.Data.Enums.OrderStatus.InProgress;
            order.VoucherId = selectVoucher;
            order.DiscountRate = Convert.ToSingle(cGiamGia);
            order.PaymentDate = order_date;

            strUrl = String.Concat(Commons.mylocalhost, "Order/add-Order");
            if (!await Commons.Add_or_UpdateAsync(order, strUrl))
                return false;

            //-- Add vào bảng OrderDetail và xóa nó khỏi giỏ hàng
            foreach (var item in prodOrders)
            {
                ProductVariation productVariation = lstProductvariation.FirstOrDefault(c => c.ProductId == new Guid(item.productId) && c.ColorId == item.color && c.SizeId == item.size);
                if (productVariation == null) return false;

                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = order_id;
                orderDetail.ProductVariationId = productVariation.Id;
                orderDetail.Price = decimal.Parse(item.price.ToString());
                orderDetail.Quantity = item.quantity;
                orderDetail.TotalMoney = decimal.Parse(item.totalMoneyItem.ToString());
                orderDetail.Status = GProject.Data.Enums.OrderDetailStatus.Watting;

                //-- Add vào OrderDetail
                if (!await Commons.Add_or_UpdateAsync(orderDetail, String.Concat(Commons.mylocalhost, "Order/add-Order-detail")))
                    return false;

                //-- Giảm số lượng còn lại của ProductVariation
                productVariation.QuantityInStock = productVariation.QuantityInStock - item.quantity;
                if (!await Commons.Add_or_UpdateAsync(productVariation, String.Concat(Commons.mylocalhost, "ProductVariation/update-ProductVariation")))
                    return false;

            }
            return true;
        }

        public async Task<List<OrderDto>> ShowMyOrder(Guid? orderid)
        {
            var lstProducts = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
            var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
            var lstBrand = await Commons.GetAll<Brand>(String.Concat(Commons.mylocalhost, "Brand/get-all-Brand"));
            var lstOrder = await Commons.GetAll<Order>(String.Concat(Commons.mylocalhost, "Order/get-all-Order"));
            var lstOrderDetail = await Commons.GetAll<OrderDetail>(String.Concat(Commons.mylocalhost, "Order/get-all-Order-detail"));
            var lstColor = await Commons.GetAll<Color>(String.Concat(Commons.mylocalhost, "Color/get-all-Color"));
            var lstSize = await Commons.GetAll<Size>(String.Concat(Commons.mylocalhost, "Size/get-all-Size"));

            var result = lstProducts
                        .Join(lstProductvariation,
                              t1 => t1.Id,
                              t2 => t2.ProductId,
                              (t1, t2) => new { t1, t2 })
                        .Join(lstBrand,
                              t12 => t12.t1.BrandId,
                              t3 => t3.Id,
                              (t12, t3) => new { t12.t1, t12.t2, t3 })
                        .Join(lstOrderDetail,
                              t123 => t123.t2.Id,
                              t4 => t4.ProductVariationId,
                              (t123, t4) => new { t123.t1, t123.t2, t123.t3, t4 })
                        .Join(lstOrder,
                              t1234 => t1234.t4.OrderId,
                              t5 => t5.Id,
                              (t1234, t5) => new { t1234.t1, t1234.t2, t1234.t3, t1234.t4, t5 })
                        .Join(lstColor,
                              t12345 => t12345.t2.ColorId,
                              t6 => t6.Id,
                              (t12345, t6) => new { t12345.t1, t12345.t2, t12345.t3, t12345.t4, t12345.t5, t6 })
                        .Join(lstSize,
                              t123456 => t123456.t2.SizeId,
                              t7 => t7.Id,
                              (t123456, t7) => new { t123456.t1, t123456.t2, t123456.t3, t123456.t4, t123456.t5, t123456.t6, t7 })
                        .Where(c => c.t5.Id == orderid)
                        .Select(result => new
                        {
                            Product = result.t1,
                            ProductVariation = result.t2,
                            Brand = result.t3,
                            OrderDetail = result.t4,
                            Order = result.t5,
                            Color = result.t6,
                            Size = result.t7
                        }).ToList();
            List<OrderDto> orders = Commons.ConverObject<List<OrderDto>>(result);
            return orders;
        }

        public async Task<(Pager pager, Tuple<List<ProductDTO>?, List<Color>?, List<Size>?, List<Brand>?> tuple)> GetDataForIndex(string prodName, Guid? category, int? brand, decimal? fPrice, decimal? tPrice, string type, int pg, int pageSize, string Keyword)
        {
            try
            {
                GProject.WebApplication.Services.ProductService pService = new GProject.WebApplication.Services.ProductService();
                var products = await pService.GetProductViewModel();
                if (!string.IsNullOrEmpty(type))
                {
                    products = products.Where(c => c.Product.ProductType.Contains(type)).ToList();
                }
                if (!string.IsNullOrEmpty(Keyword))
                {
                    products = products.Where(c => c.Product.Name.ToLower().Contains(Keyword.ToLower())
                    || c.Brand.Name.ToLower().Contains(Keyword.ToLower())
                    || c.Category.Name.ToLower().Contains(Keyword.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(prodName))
                    products = products.Where(c => c.Product.Name.ToLower().Contains(prodName.ToLower())).ToList();
                if (!string.IsNullOrEmpty(category.NullToString()) && category != Guid.Empty)
                    products = products.Where(c => c.Product.CategoryId == category).ToList();
                if (brand != -1)
                    products = products.Where(c => c.Brand.Id == brand).ToList();
                if (fPrice != -1)
                    products = products.Where(c => c.Product.Price >= fPrice).ToList();
                if (tPrice != -1)
                    products = products.Where(c => c.Product.Price <= tPrice).ToList();


                if (pg < 1)
                    pg = 1;

                int recsCount = products.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                products = products.Skip(recSkip).Take(pageSize).ToList();
                var lstColor = await Commons.GetAll<Color>(String.Concat(Commons.mylocalhost, "Color/get-all-Color"));
                var lstSize = await Commons.GetAll<Size>(String.Concat(Commons.mylocalhost, "Size/get-all-Size"));
                var lstBrand = await Commons.GetAll<Brand>(String.Concat(Commons.mylocalhost, "Brand/get-all-Brand"));
                return (pager, new Tuple<List<ProductDTO>?, List<Color>?, List<Size>?, List<Brand>?>(products, lstColor, lstSize, lstBrand));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
