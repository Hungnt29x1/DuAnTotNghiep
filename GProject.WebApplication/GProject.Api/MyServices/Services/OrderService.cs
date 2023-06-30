using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories;
using GProject.Data.MyRepositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace GProject.Api.MyServices.Services
{
    public class OrderService: IOrderService
    {
        private IOrderRepository _iOrderRepository;
        private IOrderDetailRepository _iOrderDetailRepository;
        public OrderService()
        {
            _iOrderRepository = new OrderRepository();
            _iOrderDetailRepository = new OrderDetailRepository();
        }

        public bool Create(Order cv)
        {
            if (cv == null) return false;
            if (_iOrderRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool Delete(Order cv)
        {
            if (cv == null) return false;
            var temp = _iOrderRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (_iOrderRepository.Delete(temp))
            {
                return true;
            }
            return false;
        }

        public List<Order> GetAll()
        {
            return _iOrderRepository.GetAll();
        }

        public bool Update(Order cv)
        {
            var temp = _iOrderRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (temp == null) return false;
            temp.UpdateDate = cv.UpdateDate;
            temp.Description = cv.Description;
            temp.Status = cv.Status;
            if (_iOrderRepository.Update(temp))
            {
                return true;
            }
            return false;
        }

        public bool CreateOrderDetail(OrderDetail cv)
        {
            if (cv == null) return false;
            if (_iOrderDetailRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool DeleteOrderDetail(OrderDetail cv)
        {
            var temp = _iOrderDetailRepository.GetAll().FirstOrDefault(c => c.OrderId == cv.OrderId && c.ProductVariationId == cv.ProductVariationId);
            if (temp == null) return false;
            if (_iOrderDetailRepository.Delete(temp))
                return true;
            return false;
        }

        public List<OrderDetail> GetAllOrderDetail()
        {
            return _iOrderDetailRepository.GetAll();
        }

        public bool UpdateOrderDetail(OrderDetail cv)
        {
            var temp = _iOrderDetailRepository.GetAll().FirstOrDefault(c => c.OrderId == cv.OrderId && c.ProductVariationId == cv.ProductVariationId);
            if (temp == null) return false;
            if (_iOrderDetailRepository.Update(temp))
            {
                return true;
            }
            return false;
        }
    }
}
