using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories;
using GProject.Data.MyRepositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace GProject.Api.MyServices.Services
{
    public class CartService: ICartService
    {
        private ICartRepository _iCartRepository;
        private ICartDetailRepository _iCartDetailRepository;
        public CartService()
        {
            _iCartRepository = new CartRepository();
            _iCartDetailRepository = new CartDetailRepository();
        }

        public bool Create(Cart cv)
        {
            if (cv == null) return false;
            if (_iCartRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool Delete(Cart cv)
        {
            if (cv == null) return false;
            var temp = _iCartRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (_iCartRepository.Delete(temp))
            {
                return true;
            }
            return false;
        }

        public List<Cart> GetAll()
        {
            return _iCartRepository.GetAll();
        }

        public bool Update(Cart cv)
        {
            var temp = _iCartRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (temp == null) return false;
            temp.UpdateDate = cv.UpdateDate;
            temp.Description = cv.Description;
            temp.Status = cv.Status;
            if (_iCartRepository.Update(temp))
            {
                return true;
            }
            return false;
        }

        public bool CreateCartDetail(CartDetail cv)
        {
            if (cv == null) return false;
            if (_iCartDetailRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool DeleteCartDetail(CartDetail cv)
        {
            var temp = _iCartDetailRepository.GetAll().FirstOrDefault(c => c.CartId == cv.CartId && c.ProductVariationId == cv.ProductVariationId);
            if (temp == null) return false;
            if (_iCartDetailRepository.Delete(temp))
                return true;
            return false;
        }

        public List<CartDetail> GetAllCartDetail()
        {
            return _iCartDetailRepository.GetAll();
        }

        public bool UpdateCartDetail(CartDetail cv)
        {
            var temp = _iCartDetailRepository.GetAll().FirstOrDefault(c => c.CartId == cv.CartId && c.ProductVariationId == cv.ProductVariationId);
            if (temp == null) return false;
            if (_iCartDetailRepository.Update(temp))
            {
                return true;
            }
            return false;
        }
    }
}
