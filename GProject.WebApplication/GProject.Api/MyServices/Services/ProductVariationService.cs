using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories;
using GProject.Data.MyRepositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace GProject.Api.MyServices.Services
{
    public class ProductVariationService: IProductVariationService
    {
        private IProductVariationRepository _iProductVariationRepository;

        public ProductVariationService()
        {
            _iProductVariationRepository = new ProductVariationRepository();
        }

        public bool Create(ProductVariation cv)
        {
            if (cv == null) return false;
            cv = new ProductVariation()
            {
                Id = cv.Id,
                ProductId = cv.ProductId,
                ColorId = cv.ColorId,
                SizeId = cv.SizeId,
                QuantityInStock = cv.QuantityInStock,
                Image = cv.Image,
                CreateDate = DateTime.Now,
            };
            if (_iProductVariationRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool Delete(ProductVariation cv)
        {
            if (cv == null) return false;
            var temp = _iProductVariationRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (_iProductVariationRepository.Delete(temp))
            {
                return true;
            }
            return false;
        }

        public List<ProductVariation> GetAll()
        {
            return _iProductVariationRepository.GetAll();
        }

        public bool Update(ProductVariation cv)
        {
            if (cv == null) return false;
            var temp = _iProductVariationRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
                        temp.QuantityInStock = cv.QuantityInStock;
            temp.Image = cv.Image;
            if (_iProductVariationRepository.Update(temp))
            {
                return true;
            }
            return false;
        }
    }
}
