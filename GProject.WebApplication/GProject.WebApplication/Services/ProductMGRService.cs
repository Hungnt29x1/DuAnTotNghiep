using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using GProject.WebApplication.Helpers;
using GProject.WebApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace GProject.WebApplication.Services
{
    public class ProductMGRService
    {
        private IProductVariationRepository _iProductVariationRepository;
        public static HttpContext _httpContext => new HttpContextAccessor().HttpContext;
		private IWebHostEnvironment _hostEnviroment => (IWebHostEnvironment)_httpContext.RequestServices.GetService(typeof(IWebHostEnvironment));

        public ProductMGRService()
        {
            _iProductVariationRepository = new ProductVariationRepository();
        }
		/// <summary>
		/// Hàm Lưu sản phâm
		/// </summary>
		/// <param name="Product"></param>
		/// <param name="CreateBy"></param>
		/// <returns></returns>
		public async Task<bool> Save(ProductMGRDTO Product, string CreateBy)
        {
            try
            {
                if (Product == null &&  Product.ProductVariationViewModel == null) return false;
                Guid uuid = Guid.NewGuid();

                //-- Set Product
                var productInfo = new Product();
                productInfo.Id = (Product.Id == Guid.Empty || Product.Id == null) ? uuid : Product.Id;
                productInfo.ProductCode = Commons.RandomString(10);
                productInfo.BrandId = Product.BrandId;
                productInfo.Name = Product.Name;
                productInfo.CategoryId = Product.CategoryId;
                productInfo.CreateDate = DateTime.Now;
                productInfo.UpdateDate = DateTime.Now;
                productInfo.CreateBy = CreateBy;
                productInfo.ViewCount = Product.ViewCount;
                productInfo.LikeCount = Product.LikeCount;
                productInfo.Price = Product.Price;
                productInfo.ImportPrice = Product.ImportPrice;
                productInfo.Status = Product.Status;
                productInfo.ProductType = Product.ProductType;
                if (!string.IsNullOrEmpty(Product.Description)) productInfo.Description = Product.Description;

                //-- Save Product Info
                string url = (Product.Id == Guid.Empty || Product.Id == null) ? Commons.mylocalhost + "ProductMGR/add-Product-mgr" : Commons.mylocalhost + "ProductMGR/update-Product-mgr";
                bool resultSaveProduct = await Commons.Add_or_UpdateAsync(productInfo, url);
                if (!resultSaveProduct) return false;

                //-- Set Value Productvariation 
                foreach (var colorVariation in Product.ProductVariationViewModel.Where(c => c.IsChecked == true))
                {
                    foreach (var sizeVariation in colorVariation.SizeAndStock)
                    {
                        string image = UploadFile(colorVariation);
                        var ProductVariationInfo = new ProductVariation();
                        ProductVariationInfo.Id = colorVariation.VariationId;
                        ProductVariationInfo.ProductId = productInfo.Id;
                        ProductVariationInfo.ColorId = colorVariation.Id;
                        ProductVariationInfo.SizeId = sizeVariation.Id;
                        if (!string.IsNullOrEmpty(image))
                            ProductVariationInfo.Image = image;
                        ProductVariationInfo.CreateDate = DateTime.Now;
                        ProductVariationInfo.UpdateDate = DateTime.Now;
                        ProductVariationInfo.QuantityInStock = sizeVariation.QuantityInstock.NullToInt();

                        if (ProductVariationInfo.Id == Guid.Empty || ProductVariationInfo.Id == null)
                        {
                            _iProductVariationRepository.Add(ProductVariationInfo);
                        }
                        else
                        {
                            var temp = _iProductVariationRepository.GetAll().FirstOrDefault(c => c.ProductId == ProductVariationInfo.ProductId && c.ColorId == ProductVariationInfo.ColorId && c.SizeId == ProductVariationInfo.SizeId);
                            if (temp != null)
                            {
                                temp.QuantityInStock = ProductVariationInfo.QuantityInStock;
                                if (!string.IsNullOrEmpty(ProductVariationInfo.Image))
                                {
                                    temp.Image = ProductVariationInfo.Image;
                                }
                                _iProductVariationRepository.Update(temp);
                            }
                        }
                        //string saveProductVariationUrl = (ProductVariationInfo.Id == Guid.Empty || ProductVariationInfo.Id == null) ? Commons.mylocalhost + "ProductVariation/add-ProductVariation" : Commons.mylocalhost + "ProductVariation/update-ProductVariation";
                        //bool resultSaveProductVariation = await Commons.Add_or_UpdateAsync(ProductVariationInfo, saveProductVariationUrl);
                        //saveProductVariationUrl = Commons.mylocalhost;
                    }
                }

                return resultSaveProduct;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ProductMGRDTO> GetProductViewModel()
        {
            try
            {
                var lstBrand = await Commons.GetAll<Brand>(String.Concat(Commons.mylocalhost, "Brand/get-all-Brand"));
                var lstColor = await Commons.GetAll<ProductVariationDTO>(String.Concat(Commons.mylocalhost, "Color/get-all-Color"));
                var lstSize = await Commons.GetAll<ProductSizeVariation>(String.Concat(Commons.mylocalhost, "Size/get-all-Size"));
                var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
                //-- Lấy danh sách từ api
                var lstObjs = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
                var result = new ProductMGRDTO() { ProductVariationList = lstProductvariation, ProductList = lstObjs, ProductVariationViewModel = lstColor.Where(c => c.Status == 1).ToList(), SizeList = lstSize };
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<ProductMGRDTO> GetProduct(Guid? id)
        {
            try
            {
                List<Color>? lstColor = await Commons.GetAll<Color>(String.Concat(Commons.mylocalhost, "Color/get-all-Color"));
                List<Size>? lstSize = await Commons.GetAll<Size>(String.Concat(Commons.mylocalhost, "Size/get-all-Size"));
                List<ProductVariation>? lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));
                var lstSizes = await Commons.GetAll<ProductSizeVariation>(String.Concat(Commons.mylocalhost, "Size/get-all-Size"));

                List<Product>? lstProduct = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
                ProductMGRDTO productData = new ProductMGRDTO();
                var product = lstProduct.FirstOrDefault(c => c.Id == id);
                productData.Id = id;
                productData.CategoryId = product.CategoryId;
                productData.Name = product.Name;
                productData.BrandId = product.BrandId;
                productData.CreateBy = product.CreateBy;
                productData.ViewCount = product.ViewCount;
                productData.LikeCount = product.LikeCount;
                productData.Price = product.Price;
                productData.ImportPrice = product.ImportPrice;
                productData.CreateDate = product.CreateDate;
                productData.Status = product.Status;
                productData.Description = product.Description;
                productData.ProductType = product.ProductType;

                List<ProductVariationDTO> lstPrdVariation = await Commons.GetAll<ProductVariationDTO>(String.Concat(Commons.mylocalhost, "Color/get-all-Color"));
                //-- Lấy danh sách color và Pariation 
                var DataObjects = (from x in lstColor
                              join y in lstProductvariation on x.Id equals y.ColorId
                              where y.ProductId == product.Id
                              select new { Colorss = x, ProdVariations = y }).DistinctBy(c => c.Colorss.Id).ToList();

                foreach (var item in DataObjects)
                {
                    //-- Lấy size và stock hiện tại của sản phẩm
                    List<ProductSizeVariation> SizeVariations = new List<ProductSizeVariation>();
                    var productVariations = lstProductvariation.Where(c => c.ProductId == item.ProdVariations.ProductId && c.ColorId == item.Colorss.Id).ToList();
                    //-- Lấy 
                    foreach (var itemVariation in productVariations)
                    {
                        SizeVariations.Add(new ProductSizeVariation() { Id = itemVariation.SizeId, Code = lstSize.Where(c => c.Id == itemVariation.SizeId).Select(c => c.Code).FirstOrDefault(), QuantityInstock = itemVariation.QuantityInStock });
                    }

                    ProductVariationDTO p = new ProductVariationDTO()
                    {
                        VariationId = item.ProdVariations.Id,
                        ProductId = item.ProdVariations.ProductId,
                        Id = item.Colorss.Id,
                        Name = item.Colorss.Name,
                        HEXCode = item.Colorss.HEXCode,
                        IsChecked = true,
                        Image = item.Colorss.Image,
                        ImageProduct = item.ProdVariations.Image,
                        Status = item.Colorss.Status,
                        SizeAndStock = SizeVariations
                    };

                    //-- Nếu tìm thấy item đã có color này r, thì xóa bỏ thằng mặc điịnh đi
                    var VariationItemRemove = lstPrdVariation.Where(c => c.Id == p.Id).FirstOrDefault();
                    if (VariationItemRemove != null)
                        lstPrdVariation.Remove(VariationItemRemove);
                    lstPrdVariation.Add(p);
                }

                productData.SizeList = lstSizes;
                productData.ProductVariationViewModel = lstPrdVariation;

                return productData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<bool> ChangeStatus(Guid? id)
        {
            try
            {
                List<Product>? lstProduct = await Commons.GetAll<Product>(String.Concat(Commons.mylocalhost, "ProductMGR/get-all-Product-mgr"));
                var product = lstProduct.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    if (product.Status == 0)
                        product.Status = 1;
                    else
                        product.Status = 0;
                    bool result = await Commons.Add_or_UpdateAsync(product, Commons.mylocalhost + "ProductMGR/update-Product-mgr");
                    return result;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


		//upload image
		private string UploadFile(ProductVariationDTO model)
		{
			string uniqueFileName = null;
            if (model.Image_Upload != null)
            {
				string uploadsFolder = Path.Combine(_hostEnviroment.WebRootPath, "images");
				uniqueFileName = model.Image_Upload.FileName;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					model.Image_Upload.CopyTo(fileStream);
				}
			}
			return uniqueFileName;
		}

        public string RandomString(int number)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, number)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
