using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories;
using GProject.Data.MyRepositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace GProject.Api.MyServices.Services
{
    public class ProductMGRService: IProductMGRService
    {
        private IProductRepository _iProductRepository;

        public ProductMGRService()
        {
            _iProductRepository = new ProductRepository();
        }

        public bool Create(Product cv)
        {
            if (cv == null) return false;
            if (_iProductRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool Delete(Product cv)
        {
            if (cv == null) return false;
            var temp = _iProductRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (_iProductRepository.Delete(temp))
            {
                return true;
            }
            return false;
        }

        public List<Product> GetAll()
        {
            return _iProductRepository.GetAll();
        }

        public bool Update(Product cv)
        {
            if (cv == null) return false;
            var temp = _iProductRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            temp.CategoryId = cv.CategoryId;
            temp.CreateBy = cv.CreateBy;
            temp.Status = cv.Status;
            temp.LikeCount = cv.LikeCount;
            temp.ViewCount = cv.ViewCount;
            temp.Description = cv.Description;
            temp.Name = cv.Name;
            temp.Price = cv.Price;
			temp.ProductType = cv.ProductType;
			temp.ImportPrice = cv.ImportPrice;
            if (_iProductRepository.Update(temp))
            {
                return true;
            }
            return false;
        }

        public bool UpdateStatus(Guid id)
        {
            if (id == null) return false;
            var temp = _iProductRepository.GetAll().FirstOrDefault(c => c.Id == id);
            
            if(temp.Status == 0)
            {
                temp.Status = 1;
            }
            else{
                temp.Status = 0;
            }
            temp.UpdateDate = DateTime.Now;
            return false;
        }
    }
}
