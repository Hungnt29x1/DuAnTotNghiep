using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using GProject.WebApplication.Helpers;
using GProject.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Xml.Linq;

namespace GProject.WebApplication.Services
{
    public class ProductService
    {
        private static ICartDetailRepository cartDetailRepository;
        public ProductService()
        {
            cartDetailRepository = new CartDetailRepository();
        }
        public async Task<List<ProductDTO>> GetProductViewModel()
        {
            //var result = _product_DetailRepository.GetAll().Join(_productRepository.GetAll(), a => a.Product_Id, b => b.Id, (a, b) => new { a, b }).Join(_colorRepository.GetAll(), c => c.a.Color_Id, d => d.Id, (c, d) => new { c, d }).Join(_categoryRepository.GetAll(), e => e.c.a.Category_Id, f => f.Id, (e, f) => new { e, f }).Join(_producerRepository.GetAll(), g => g.e.c.a.Producer_Id, h => h.Id, (g, h) => new { g, h }).Select(i => new { Producer = i.h, Category = i.g.f, Color = i.g.e.d, Product = i.g.e.c.b, Product_Detail = i.g.e.c.a });
            var lstBrand = await Commons.GetAll<Brand>(String.Concat(Commons.mylocalhost, "Brand/get-all-Brand"));
            var lstCategory = await Commons.GetAll<Category>(String.Concat(Commons.mylocalhost, "Category/get-all-Category"));
            var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
            var lstProducts = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
            var data = lstProducts.Where(x => x.Status != 1)
               .Join(lstBrand, a => a.BrandId, b => b.Id, (a, b) => new { a, b })
               .Join(lstCategory, ab => ab.a.CategoryId, c => c.Id, (ab, c) => new { ab, c })
               .Select(i => new
               {
                   Product = i.ab.a,
                   Brand = i.ab.b,
                   Category = i.c,
                   ProductVariations = lstProductvariation.Where(c => c.ProductId == i.ab.a.Id).ToList()
               }).Where(x => x.Brand.Status == 1 && x.Category.Status == 1 && x.Product.Status == 0);
            return Commons.ConverObject<List<ProductDTO>>(data);
        }

        public async Task<(Pager pager, Tuple<List<ProductDTO>?, List<Color>?, List<Size>?, List<Brand>?, List<ViewHistory>?> tuple)> GetDataForIndex(string prodName, Guid? category, int? brand, decimal? fPrice, decimal? tPrice, string type, int pg, int pageSize, Customer customer, string Keyword)
        {
            var products = await GetProductViewModel();
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
            var lstViewHistory = await Commons.GetAll<ViewHistory>(String.Concat(Commons.mylocalhost, "ViewHistory/get-all-ViewHistory"));

            if (customer != null)
            {
                lstViewHistory = lstViewHistory.Where(c => c.CustomerId == customer.Id).OrderByDescending(c => c.DateView).Take(10).ToList();
			}
            return (pager, new Tuple<List<ProductDTO>?, List<Color>?, List<Size>?, List<Brand>?, List<ViewHistory>?>(products, lstColor, lstSize, lstBrand, lstViewHistory));
        }

        public async Task<Tuple<Product?, List<ProductVariation>?, Brand?, EvaluateCommentDTO, decimal, int, Customer>> GetProductDetail(Guid product_id, Customer customer)
        {
            decimal ratingSum = 0;
            int ratingCount = 0;

            //-- Lấy danh sách sản phẩm
            var lstProducts = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
            var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
            var lstBrand = await Commons.GetAll<Brand>(String.Concat(Commons.mylocalhost, "Brand/get-all-Brand"));
            var lstEvaluate = await Commons.GetAll<Evaluate>(String.Concat(Commons.mylocalhost, "Evaluate/get-all-Evaluate"));
            var lstViewHistory = await Commons.GetAll<ViewHistory>(String.Concat(Commons.mylocalhost, "ViewHistory/get-all-ViewHistory"));

            var product = lstProducts.FirstOrDefault(c => c.Id == product_id);
            if (product != null)
            {
                product.ViewCount = product.ViewCount + 1;
                await Commons.Add_or_UpdateAsync(product, String.Concat(Commons.mylocalhost, "ProductMGR/update-Product-mgr"));
            }

            var productVariation = lstProductvariation.Where(c => c.ProductId == product_id).ToList();

            //-- Đánh giá
            EvaluateCommentDTO evaDTO= new EvaluateCommentDTO();
            evaDTO.Title = product.Name;
            evaDTO.ProductId = product_id;
            List<Evaluate> evaluates = new List<Evaluate>();
            var Evalutes = lstEvaluate.Where(c => c.ProductId == product_id).ToList();
            if (Evalutes != null)
            {
                evaluates = Evalutes;
            }
            evaDTO.Evaluates = evaluates;

            var ratings = lstEvaluate.Where(c => c.ProductId == product_id).ToList();
            if (ratings.Count() > 0)
            {
                ratingSum = ratings.Sum(c => c.Rating).NullToDecimal();
                ratingCount = ratings.Count();
            }
            else
            {
                ratingSum = 0;
                ratingCount = 0;
            }

            //--Thêm vaofg danh sách xem gần đây của Khách hàng này
            ViewHistory viewHistory = new ViewHistory()
            {
                DateView = DateTime.Now,
                ProductId = product_id,
                CustomerId = customer.Id
            };
            if (lstViewHistory.FirstOrDefault(c => c.CustomerId == customer.Id && c.ProductId == product_id) != null)
                await Commons.Add_or_UpdateAsync(viewHistory, String.Concat(Commons.mylocalhost, "ViewHistory/update-ViewHistory"));
            else
                await Commons.Add_or_UpdateAsync(viewHistory, String.Concat(Commons.mylocalhost, "ViewHistory/add-ViewHistory"));


            return new Tuple<Product?, List<ProductVariation>?, Brand?, EvaluateCommentDTO, decimal, int, Customer>(product,
                    productVariation,
                    lstBrand.FirstOrDefault(c => c.Id == product.BrandId), evaDTO, ratingSum, ratingCount, customer);
        }

        public async Task<bool> AddToCart(string cTotalMoney, string cColor, string cSize, string cQuantity, string cPrice, string cProductId, string cDescription, Guid? customer_id)
        {
            //-- Lấy danh sách dữ liệu
            var lstCart = await Commons.GetAll<Cart>(String.Concat(Commons.mylocalhost, "Cart/get-all-Cart"));
            var lstCartDetail = await Commons.GetAll<CartDetail>(String.Concat(Commons.mylocalhost, "Cart/get-all-cart-detail"));
            var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
            string strUrl = "";
            Guid cart_id = Guid.NewGuid();

            //-- Kiểm tra xem khách hàng này đã tạo giỏ hàng hay chưa, nếu chưa => tạo giỏ hàng
            Cart cart_data = lstCart.FirstOrDefault(c => c.CustomerId == customer_id);
            if (cart_data == null)
            {
                cart_data = new Cart();
                cart_data.Id = cart_id;
                cart_data.CustomerId = customer_id;
                cart_data.CartId = string.Concat(DateTime.Now.ToString("ddMMyy"), Commons.RandomString(9));
                cart_data.CreateDate = DateTime.Now;
                cart_data.UpdateDate = DateTime.Now;
                cart_data.Status = 0;
                cart_data.Description = cDescription;

                strUrl = String.Concat(Commons.mylocalhost, "Cart/add-Cart");
            }
            else
            {
                cart_data.UpdateDate = DateTime.Now;
                strUrl = String.Concat(Commons.mylocalhost, "Cart/update-Cart");
            }

            if (!await Commons.Add_or_UpdateAsync(cart_data, strUrl))
                return false;

            ProductVariation productVariation = lstProductvariation.FirstOrDefault(c => c.ProductId == new Guid(cProductId) && c.ColorId == int.Parse(cColor) && c.SizeId == int.Parse(cSize));
            if (productVariation == null) return false;

            //-- Kiểm tra xem sản phẩm này || variation này đã tồn tại trong giỏ hàng của người này hay chưa
            CartDetail? cartdetail_data = lstCartDetail.FirstOrDefault(c => c.CartId == cart_data.Id
                                        && c.ProductVariationId == productVariation.Id
                                        && c.Price == decimal.Parse(cPrice));

            DateTime timeout = DateTime.Now;

            if (cartdetail_data == null)
            {
                cartdetail_data = new CartDetail();
                cartdetail_data.CartId = cart_data.Id;
                cartdetail_data.ProductVariationId = productVariation.Id;
                cartdetail_data.Price = decimal.Parse(cPrice);
                cartdetail_data.Quantity = int.Parse(cQuantity);
                cartdetail_data.CreateDate = DateTime.Now;
                cartdetail_data.ToatlMoney = decimal.Parse(cTotalMoney);
                cartdetail_data.Status = Data.Enums.CartDetailStatus.Ready;
                cartdetail_data.Description = cDescription.NullToString();
                cartdetail_data.TimeOut = timeout.AddMinutes(15);
                strUrl = String.Concat(Commons.mylocalhost, "Cart/add-cart-detail");
                cartDetailRepository.Add(cartdetail_data);
            }
            else
            {
                cartdetail_data.Quantity = cartdetail_data.Quantity + cQuantity.NullToInt();
                cartdetail_data.ToatlMoney = cartdetail_data.ToatlMoney + cTotalMoney.NullToDecimal();
                cartdetail_data.Description = cDescription.NullToString();
                cartdetail_data.TimeOut = timeout.AddMinutes(15);
                strUrl = String.Concat(Commons.mylocalhost, "Cart/update-cart-detail");
                cartDetailRepository.Update(cartdetail_data);
            }

            //-- Giảm số lượng còn lại của ProductVariation
            productVariation.QuantityInStock = productVariation.QuantityInStock - int.Parse(cQuantity);
            if (!await Commons.Add_or_UpdateAsync(productVariation, String.Concat(Commons.mylocalhost, "ProductVariation/update-ProductVariation")))
                return false;

            //if (!await Commons.Add_or_UpdateAsync(cartdetail_data, strUrl))
            //    return false;

            return true;
        }

        
        public async Task<bool> ReleaseHeart(string cProductId)
        {
            var lstProducts = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
            var product = lstProducts.FirstOrDefault(c => c.Id == new Guid(cProductId));
            bool result = true;
            if (product != null)
            {
                product.LikeCount = product.LikeCount + 1;
                result = await Commons.Add_or_UpdateAsync(product, String.Concat(Commons.mylocalhost, "ProductMGR/update-Product-mgr"));

            }
            return result;
        }

        public async Task<int> GetQuantityInstock(string cColorId, string cSizeId, string cProductId)
        {
            var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
            if (!string.IsNullOrEmpty(cColorId) && !string.IsNullOrEmpty(cSizeId))
            {
                return lstProductvariation.Where(c => c.ProductId == new Guid(cProductId) && c.SizeId == int.Parse(cSizeId) && c.ColorId == int.Parse(cColorId)).Select(c => c.QuantityInStock).FirstOrDefault().NullToInt();
            }
            
            if (!string.IsNullOrEmpty(cColorId) && string.IsNullOrEmpty(cSizeId))
            {
                return lstProductvariation.Where(c => c.ProductId == new Guid(cProductId) && c.ColorId == int.Parse(cColorId)).Sum(c => c.QuantityInStock);
            }

            if (string.IsNullOrEmpty(cColorId) && !string.IsNullOrEmpty(cSizeId))
            {
                return lstProductvariation.Where(c => c.ProductId == new Guid(cProductId) && c.SizeId == int.Parse(cSizeId)).Sum(c => c.QuantityInStock);
            }
            return lstProductvariation.Where(c => c.ProductId == new Guid(cProductId)).Sum(c => c.QuantityInStock);
        }

        public async Task<List<CartDTO>> ShowMyCart(Guid? customer_id, string prodName, decimal? fPrice, decimal? tPrice, int? brand)
        {
			var lstProducts = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
			var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
			var lstBrand = await Commons.GetAll<Brand>(String.Concat(Commons.mylocalhost, "Brand/get-all-Brand"));
			var lstCart = await Commons.GetAll<Cart>(String.Concat(Commons.mylocalhost, "Cart/get-all-Cart"));
			var lstCartDetail = await Commons.GetAll<CartDetail>(String.Concat(Commons.mylocalhost, "Cart/get-all-cart-detail"));
			var lstColor = await Commons.GetAll<Color>(String.Concat(Commons.mylocalhost, "Color/get-all-Color"));
			var lstSize = await Commons.GetAll<Size>(String.Concat(Commons.mylocalhost, "Size/get-all-Size"));
            var lstOrderDetails = await Commons.GetAll<OrderDetail>(String.Concat(Commons.mylocalhost, "Order/get-all-Order-detail"));

            var result = lstProducts
						.Join(lstProductvariation,
							  t1 => t1.Id,
							  t2 => t2.ProductId,
							  (t1, t2) => new { t1, t2 })
						.Join(lstBrand,
							  t12 => t12.t1.BrandId,
							  t3 => t3.Id,
							  (t12, t3) => new { t12.t1, t12.t2, t3 })
						.Join(lstCartDetail,
							  t123 => t123.t2.Id,
							  t4 => t4.ProductVariationId,
							  (t123, t4) => new { t123.t1, t123.t2, t123.t3, t4 })
						.Join(lstCart,
							  t1234 => t1234.t4.CartId,
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
                        
                        .Where(c => c.t5.CustomerId == customer_id)
						.Select(result => new {
							Product = result.t1,
							ProductVariation = result.t2,
							Brand = result.t3,
							CartDetail = result.t4,
							Cart = result.t5,
							Color = result.t6,
							Size = result.t7
						}).ToList();
			List<CartDTO> carts = Commons.ConverObject<List<CartDTO>>(result);
			if (!string.IsNullOrEmpty(prodName))
				carts = carts.Where(c => c.Product.Name.ToLower().Contains(prodName.ToLower())).ToList();
			if (brand != -1)
				carts = carts.Where(c => c.Brand.Id == brand).ToList();
			if (fPrice != -1)
				carts = carts.Where(c => c.Product.Price >= fPrice).ToList();
			if (tPrice != -1)
				carts = carts.Where(c => c.Product.Price <= tPrice).ToList();

			return carts.OrderByDescending(x => x.CartDetail.CreateDate).ToList();
        }

        public async Task<List<ShowOrderDto>> ShowMyOrder(Guid? customer_id)
        {
            var lstProducts = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
            var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
            var lstBrand = await Commons.GetAll<Brand>(String.Concat(Commons.mylocalhost, "Brand/get-all-Brand"));           
            var lstColor = await Commons.GetAll<Color>(String.Concat(Commons.mylocalhost, "Color/get-all-Color"));
            var lstSize = await Commons.GetAll<Size>(String.Concat(Commons.mylocalhost, "Size/get-all-Size"));
            var lstOrderDetails = await Commons.GetAll<OrderDetail>(String.Concat(Commons.mylocalhost, "Order/get-all-Order-detail"));
            var lstOrder = await Commons.GetAll<Order>(String.Concat(Commons.mylocalhost, "Order/get-all-Order"));

            var result = lstProducts
                        .Join(lstProductvariation,
                              t1 => t1.Id,
                              t2 => t2.ProductId,
                              (t1, t2) => new { t1, t2 })
                        .Join(lstBrand,
                              t12 => t12.t1.BrandId,
                              t3 => t3.Id,
                              (t12, t3) => new { t12.t1, t12.t2, t3 })
                        .Join(lstOrderDetails,
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
                        .Select(result => new {
                            Product = result.t1,
                            ProductVariation = result.t2,
                            Brand = result.t3,
                            OrderDetail = result.t4,
                            Order = result.t5,
                            Color = result.t6,
                            Size = result.t7
                        }).ToList();
            if (customer_id != null)
            {
                result = result.Where(x => x.Order.CustomerId == customer_id).ToList();
            }
            List<ShowOrderDto> showOrder = Commons.ConverObject<List<ShowOrderDto>>(result);
            

            return showOrder;
        }

        public async Task<List<CartDTO>> ShowMyCartNotLogin(List<Cart> cart, List<CartDetail> cartDetails)
        {
            var lstProducts = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
            var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
            var lstBrand = await Commons.GetAll<Brand>(String.Concat(Commons.mylocalhost, "Brand/get-all-Brand"));
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
                        .Join(cartDetails,
                              t123 => t123.t2.Id,
                              t4 => t4.ProductVariationId,
                              (t123, t4) => new { t123.t1, t123.t2, t123.t3, t4 })
                        .Join(cart,
                              t1234 => t1234.t4.CartId,
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

                        .Select(result => new {
                            Product = result.t1,
                            ProductVariation = result.t2,
                            Brand = result.t3,
                            CartDetail = result.t4,
                            Cart = result.t5,
                            Color = result.t6,
                            Size = result.t7
                        }).ToList();
            List<CartDTO> carts = Commons.ConverObject<List<CartDTO>>(result);

            return carts.OrderByDescending(x => x.CartDetail.CreateDate).ToList();
        }
    }
}
