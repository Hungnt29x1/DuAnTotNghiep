using GProject.Api.MyServices.IServices;
using GProject.Api.MyServices.Services;
using GProject.Data.DomainClass;
using GProject.WebApplication.Helpers;
using GProject.WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GProject.WebApplication.Services;
using GProject.WebApplication.Models.DeliveryAddressAndShippingFee;
using GProject.Data.MyRepositories.IRepositories;

namespace GProject.WebApplication.Controllers
{
	public class ProductController : Controller
	{
		private readonly DeliveryAddressAndShippingFeeService _deliveryAddressAndShippingFeeService;
		public ProductController()
		{
			_deliveryAddressAndShippingFeeService = new DeliveryAddressAndShippingFeeService();
		}


		private static string _prodName;
		private static Guid? _category;
		private static int? _brand;
		private static decimal? _fPrice;
		private static decimal? _tPrice;


		//[HttpGet]
		public async Task<ActionResult> Index(
			string? prodName,
			Guid? category,
			int? brand,
			decimal? fPrice,
			decimal? tPrice,
			int? type,
			string? clear,
			int pg = 1,
			int pageSize = 10,
			string Keyword = null
		)
		{
			try
			{
				if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() != "customer")
					return RedirectToAction("AccessDenied", "Account");
				decimal valFromPrice = fPrice.HasValue ? fPrice.Value : -1;
				decimal valToPrice = tPrice.HasValue ? tPrice.Value : -1;
				Guid valCategory = category.HasValue ? category.Value : Guid.Empty;
				int valBrand = brand.HasValue ? brand.Value : -1;

				SetDataStatic(prodName, category, brand, fPrice, tPrice);

				if (prodName == null)
					prodName = _prodName;

				if (category == null)
					category = _category;

				if (brand == null)
					brand = _brand;

				if (fPrice == null)
					fPrice = _fPrice;

				if (tPrice == null)
					tPrice = _tPrice;

				if (clear != null)
				{
					prodName = null;
					_prodName = null;
					category = null;
					_category = null;
					brand = null;
					_brand = null;
					fPrice = null;
					_fPrice = null;
					tPrice = null;
					_tPrice = null;
				}

				var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
				GProject.WebApplication.Services.ProductService pService = new GProject.WebApplication.Services.ProductService();
				var data = await pService.GetDataForIndex(prodName, valCategory, valBrand, valFromPrice, valToPrice, type.NullToString(), pg, pageSize, customer, Keyword);
				this.ViewBag.Pager = data.pager;
				ViewData["pg"] = pg;
				this.ViewData[nameof(prodName)] = (object)prodName;
				this.ViewData[nameof(fPrice)] = (object)valFromPrice;
				this.ViewData[nameof(tPrice)] = (object)valFromPrice;
				this.ViewData[nameof(brand)] = (object)valBrand;
				this.ViewData[nameof(category)] = (object)valCategory;
				this.ViewData[nameof(Keyword)] = (object)Keyword;
				return View(data.tuple);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return RedirectToAction("AccessDenied", "Account");
			}
		}

		[Route("/productdetail/{product_id}")]
		public async Task<ActionResult> ProductDetail(Guid product_id)
		{
			try
			{
				if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() != "customer")
					return RedirectToAction("AccessDenied", "Account");
				GProject.WebApplication.Services.ProductService pService = new GProject.WebApplication.Services.ProductService();
				if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
					ViewData["Mess"] = HttpContext.Session.GetString("mess");
				HttpContext.Session.Remove("mess");
				var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
				if (customer == null)
					customer = new Customer();
				ViewBag.productId = product_id;
				return View(await pService.GetProductDetail(product_id, customer));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return RedirectToAction("AccessDenied", "Account");
			}
		}

		//[GProject.WebApplication.Services.Authorize]
		public async Task<ActionResult> AddToCart(string cTotalMoney, string cColor, string cSize, string cQuantity, string cPrice, string cProductId, string cDescription)
		{
			try
			{
				//if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() != "customer")
				//    return RedirectToAction("AccessDenied", "Account");
				var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");

				//Tao gio hang khi chua login
				if (customer == null)
				{
					bool checkAddCartDetail = true;

					var cartNotLogin = HttpContext.Session.GetObjectFromJson<Cart>("Create_CartNotLogin");
					List<CartDetail> cart_Details = HttpContext.Session.GetObjectFromJson<List<CartDetail>>("add_cart_details");

					if (cartNotLogin == null)
					{
						var create_Cart = Create_CartNotLogin();

						checkAddCartDetail = await AddToCart_DetailsNotLogin(
							cart_Details,
							create_Cart.Id,
							cTotalMoney,
							cColor,
							cSize,
							cQuantity,
							cPrice,
							cProductId,
							cDescription
							);
					}
					else
					{
						checkAddCartDetail = await AddToCart_DetailsNotLogin(
							cart_Details,
							cartNotLogin.Id,
							cTotalMoney,
							cColor,
							cSize,
							cQuantity,
							cPrice,
							cProductId,
							cDescription
						   );
					}

					if (!checkAddCartDetail)
						HttpContext.Session.SetString("mess", "Failed");
					else
						HttpContext.Session.SetString("mess", "Success");
					return RedirectToAction("ProductDetail", "Product", new { product_id = new Guid(cProductId) });
				}

				GProject.WebApplication.Services.ProductService pService = new GProject.WebApplication.Services.ProductService();

				bool result = await pService.AddToCart(cTotalMoney, cColor, cSize, cQuantity, cPrice, cProductId, cDescription, customer.Id);

				if (!result)
					HttpContext.Session.SetString("mess", "Failed");
				else
					HttpContext.Session.SetString("mess", "Success");
				return RedirectToAction("ProductDetail", "Product", new { product_id = new Guid(cProductId) });
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return RedirectToAction("AccessDenied", "Account");
			}
		}



		//[GProject.WebApplication.Services.Authorize]
		[HttpGet]
		public ActionResult Order(string products)
		{
			//if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() != "customer")
			//    return RedirectToAction("AccessDenied", "Account");
			List<ProdOrder> lstProdOrders = JsonConvert.DeserializeObject<List<ProdOrder>>(products);
			Commons.setObjectAsJson(HttpContext.Session, "productOrders", lstProdOrders);
			// Xử lý danh sách đối tượng ở đây
			return Ok();
		}

		//[GProject.WebApplication.Services.Authorize]
		[HttpGet]
		public async Task<ActionResult> RemoveToCart(string products)
		{
			var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
			var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
			List<ProdOrder> lstProdOrders = JsonConvert.DeserializeObject<List<ProdOrder>>(products);

			if (customer == null)
			{
				List<CartDetail> cart_Details = HttpContext.Session.GetObjectFromJson<List<CartDetail>>("add_cart_details");
				foreach (var item in lstProdOrders)
				{
					var delete_cart_Details = cart_Details.FirstOrDefault(x => x.ProductVariationId.ToString() == item.prodVariationId);
					if (delete_cart_Details != null)
						cart_Details.Remove(delete_cart_Details);
				}

				Commons.setObjectAsJson(HttpContext.Session, "add_cart_details", cart_Details);
				return Ok();
				//return RedirectToAction("AccessDenied", "Account");
			}

			foreach (var item in lstProdOrders)
			{
				ProductVariation productVariation = lstProductvariation.FirstOrDefault(c => c.Id == new Guid(item.prodVariationId));
				productVariation.QuantityInStock = productVariation.QuantityInStock + item.quantity;
				await Commons.Add_or_UpdateAsync(productVariation, String.Concat(Commons.mylocalhost, "ProductVariation/update-ProductVariation"));

				string urlRemoveCartDetail = string.Concat(Commons.mylocalhost, "Cart/delete-cart-detail?id=", new Guid(item.cartId), "&productVariation_id=", new Guid(item.prodVariationId));
				var RestRemoveCartDetail = new RestSharpHelper(urlRemoveCartDetail);
				var resRemoveCartDetail = await RestRemoveCartDetail.RequestBaseAsync(urlRemoveCartDetail, RestSharp.Method.Delete);
				if (!resRemoveCartDetail.IsSuccessful)
				{
					HttpContext.Session.SetString("mess", "Failed");
					return RedirectToAction("AccessDenied", "Account");
				}
			}
			HttpContext.Session.SetString("mess", "Success");
			var productId = lstProductvariation.Where(c => c.Id == new Guid(lstProdOrders.Select(c => c.prodVariationId).FirstOrDefault())).Select(c => c.ProductId).FirstOrDefault();
			// Xử lý danh sách đối tượng ở đây
			return Ok();
		}

		[GProject.WebApplication.Services.Authorize]
		[HttpGet]
		public ActionResult GetCustomerInfo()
		{
			var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
			return Ok(customer);
		}

		[GProject.WebApplication.Services.Authorize]
		[HttpGet]
		public async Task<ActionResult> ReleaseHeart(string cProductId)
		{
			try
			{
				GProject.WebApplication.Services.ProductService pService = new GProject.WebApplication.Services.ProductService();
				await pService.ReleaseHeart(cProductId);
				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return RedirectToAction("AccessDenied", "Account");
			}
		}

		//[GProject.WebApplication.Services.Authorize]
		[HttpGet]
		public async Task<ActionResult> GetQuantityInstock(string cColorId, string cSizeId, string cProductId)
		{
			GProject.WebApplication.Services.ProductService pService = new GProject.WebApplication.Services.ProductService();
			int instock = await pService.GetQuantityInstock(cColorId.NullToString(), cSizeId.NullToString(), cProductId);
			return Ok(instock);
		}

		//[GProject.WebApplication.Services.Authorize]
		public async Task<ActionResult> ShowDetailMyCart(string prodName, int? brand, decimal? fPrice, decimal? tPrice, int idProvince)
		{
			try
			{
				//if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() != "customer")
				//    return RedirectToAction("AccessDenied", "Account");

				GProject.WebApplication.Services.ProductService pService = new GProject.WebApplication.Services.ProductService();
				var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");

				decimal valFromPrice = fPrice.HasValue ? fPrice.Value : -1;
				decimal valToPrice = tPrice.HasValue ? tPrice.Value : -1;
				int valBrand = brand.HasValue ? brand.Value : -1;

				this.ViewData[nameof(prodName)] = (object)prodName;
				this.ViewData[nameof(fPrice)] = (object)valFromPrice;
				this.ViewData[nameof(tPrice)] = (object)valFromPrice;
				this.ViewData[nameof(brand)] = (object)valBrand;

				if (customer == null)
				{
					var cartNotLogin = HttpContext.Session.GetObjectFromJson<Cart>("Create_CartNotLogin");
					if (cartNotLogin == null) return View(new List<CartDTO>());
					List<Cart> cartList = new List<Cart>()
					{
						new Cart()
						{
							Id = cartNotLogin.Id,
							CartId = cartNotLogin.CartId,
							CreateDate = cartNotLogin.CreateDate,
							Status = cartNotLogin.Status,
						}

					};
					List<CartDetail> cart_Details = HttpContext.Session.GetObjectFromJson<List<CartDetail>>("add_cart_details");

					return View(await pService.ShowMyCartNotLogin(cartList, cart_Details));
				}




				if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
					ViewData["Mess"] = HttpContext.Session.GetString("mess");
				HttpContext.Session.Remove("mess");

				return View(await pService.ShowMyCart(customer.Id, prodName, valFromPrice, valToPrice, valBrand));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return RedirectToAction("AccessDenied", "Account");
			}
		}

		public ActionResult ProdNomal()
		{
			return RedirectToAction("Index", "Product", new { type = 0 });
		}
		public ActionResult ProdNew()
		{
			return RedirectToAction("Index", "Product", new { type = 1 });
		}
		public ActionResult ProdFeatured()
		{
			return RedirectToAction("Index", "Product", new { type = 2 });
		}
		public ActionResult ProdFavorite()
		{
			return RedirectToAction("Index", "Product", new { type = 3 });
		}
		public ActionResult ProdPromotional()
		{
			return RedirectToAction("Index", "Product", new { type = 4 });
		}
		[HttpGet]
		public async Task<JsonResult> ShippingFee(int district_id, string ward_code)
		{
			var fee = await _deliveryAddressAndShippingFeeService.ShippingFee(district_id, ward_code);
			return Json(fee);
		}

		[HttpGet]
		public async Task<JsonResult> GetDataDeliveryAddress(int id)
		{
			var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
			var lstObjs = await Commons.GetAll<DeliveryAddress>(String.Concat(Commons.mylocalhost, "DeliveryAddress/get-all"));
			var data = lstObjs.FirstOrDefault(x => x.CustomerId == customer.Id && x.Id == id);
			return Json(data);
		}

		[HttpGet]
		public async Task<JsonResult> GetDataDeliveryAddressByCustomerId()
		{
			var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
			var lstObjs = await Commons.GetAll<DeliveryAddress>(String.Concat(Commons.mylocalhost, "DeliveryAddress/get-all"));
			var data = lstObjs.Where(x => x.CustomerId == customer.Id);
			return Json(data);
		}

		[HttpGet]
		public async Task<JsonResult> GetVoucher(int? totalMoneyOrder, int? id, string? discountCode)
		{
			var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
			var lstVoucher = await Commons.GetAll<Voucher>(String.Concat(Commons.mylocalhost, "Voucher/get-all"));
			if (id == null && totalMoneyOrder != null && discountCode == null)
			{
				var data = lstVoucher.Where(x => x.Status == 1 && x.ExpirationDate >= DateTime.Now && x.MinimumOrder <= totalMoneyOrder && x.NumberOfVouchers > 0);
				return Json(data);
			}
			else if (id != null && totalMoneyOrder == null && discountCode == null)
			{
				var data = lstVoucher.FirstOrDefault(x => x.Id == id && x.NumberOfVouchers > 0 && x.Status != 0 && x.ExpirationDate >= DateTime.Now);
				return Json(data);
			}
			else if (id == null && totalMoneyOrder == null && discountCode != null)
			{
				var data = lstVoucher.FirstOrDefault(x => x.VoucherId == discountCode && x.NumberOfVouchers > 0 && x.Status != 0 && x.ExpirationDate >= DateTime.Now);
				return Json(data);
			}
			else
			{
				return Json(null);
			}
		}

		public Cart Create_CartNotLogin()
		{
			Guid cart_id = Guid.NewGuid();

			//-- Tạo giỏ hàng

			Cart cart_data = new Cart();
			cart_data.Id = cart_id;
			cart_data.CartId = string.Concat(DateTime.Now.ToString("ddMMyy"), Commons.RandomString(9));
			cart_data.CreateDate = DateTime.Now;
			cart_data.Status = 0;

			Commons.setObjectAsJson(HttpContext.Session, "Create_CartNotLogin", cart_data);

			return cart_data;
		}

		public async Task<bool> AddToCart_DetailsNotLogin(
			List<CartDetail> cart_Deatils,
			Guid? cartId,
			string cTotalMoney,
			string cColor,
			string cSize,
			string cQuantity,
			string cPrice,
			string cProductId,
			string cDescription
		)
		{

			try
			{
				if (cart_Deatils == null) cart_Deatils = new List<CartDetail>();

				//-- Lấy danh sách dữ liệu
				var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));


				ProductVariation productVariation = lstProductvariation.FirstOrDefault(c => c.ProductId == new Guid(cProductId) && c.ColorId == int.Parse(cColor) && c.SizeId == int.Parse(cSize));
				if (productVariation == null) return false;

				//-- Kiểm tra xem sản phẩm này || variation này đã tồn tại trong giỏ hàng của người này hay chưa
				CartDetail? cartdetail_data = null;

				if (cart_Deatils.Count() != 0)
				{
					cartdetail_data = cart_Deatils.FirstOrDefault(c =>
												c.ProductVariationId == productVariation.Id
												&& c.Price == decimal.Parse(cPrice)
												);
				}

				if (cartdetail_data == null)
				{
					cartdetail_data = new CartDetail();
					cartdetail_data.CartId = cartId;
					cartdetail_data.ProductVariationId = productVariation.Id;
					cartdetail_data.Price = decimal.Parse(cPrice);
					cartdetail_data.Quantity = int.Parse(cQuantity);
					cartdetail_data.CreateDate = DateTime.Now;
					cartdetail_data.ToatlMoney = decimal.Parse(cTotalMoney);
					cartdetail_data.Status = Data.Enums.CartDetailStatus.Ready;
					cartdetail_data.Description = cDescription.NullToString();

					cart_Deatils.Add(cartdetail_data);
				}
				else
				{
					cartdetail_data.Quantity = cartdetail_data.Quantity + cQuantity.NullToInt();
					cartdetail_data.ToatlMoney = cartdetail_data.ToatlMoney + cTotalMoney.NullToDecimal();
					cartdetail_data.Description = cDescription.NullToString();

					for (int i = 0; i < cart_Deatils.Count; i++)
					{
						if (cart_Deatils[i].CartId == cartdetail_data.CartId)
						{
							cart_Deatils[i] = cartdetail_data;//Gán lại giá trị mới
						}
					}
				}

				Commons.setObjectAsJson(HttpContext.Session, "add_cart_details", cart_Deatils);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void SetDataStatic(string? prodName, Guid? category, int? brand, decimal? fPrice, decimal? tPrice)
		{
			if (prodName != null)
				_prodName = prodName;

			if (category != null)
				_category = category;

			if (brand != null)
				_brand = brand;

			if (fPrice != null)
				_fPrice = fPrice;

			if (tPrice != null)
				_tPrice = tPrice;
		}
	}
}