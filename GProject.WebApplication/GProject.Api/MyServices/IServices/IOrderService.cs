using GProject.Data.DomainClass;
using System.Collections.Generic;

namespace GProject.Api.MyServices.IServices
{
    public interface IOrderService
    {
        public bool Create(Order cv);

        public bool Update(Order cv);

        public bool Delete(Order cv);

        public List<Order> GetAll();

        public bool CreateOrderDetail(OrderDetail cv);

        public bool DeleteOrderDetail(OrderDetail cv);

        public List<OrderDetail> GetAllOrderDetail();

        public bool UpdateOrderDetail(OrderDetail cv);
    }
}
