using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GProject.Data.DomainClass;
using System.Collections.Generic;
using GProject.Api.MyServices.IServices;
using GProject.Api.MyServices.Services;
using System;
using System.Linq;
using System.Drawing;

namespace GProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService iOrderService;
        public OrderController()
        {
            iOrderService = new OrderService();
        }

        [HttpGet]
        [Route("get-all-Order")]
        public List<GProject.Data.DomainClass.Order> GetOrder()
        {
            return iOrderService.GetAll();
        }

        [HttpPost]
        [Route("add-Order")]
        public bool AddOrder([FromBody] GProject.Data.DomainClass.Order Order)
        {
            return iOrderService.Create(Order);
        }

        [HttpPost]
        [Route("update-Order")]
        public bool UpdateOrder([FromBody] GProject.Data.DomainClass.Order Order)
        {
            return iOrderService.Update(Order);
        }

        [HttpGet]
        [Route("order-canceled")]
        public bool OrderCanceled(Guid id)
        {
            var order = iOrderService.GetAll().FirstOrDefault(x => x.Id == id);
            order.Status = Data.Enums.OrderStatus.Canceled;
            order.UpdateDate = DateTime.Now;
            return iOrderService.Update(order);
        }

        [HttpGet]
        [Route("order-accomplished")]
        public bool OrderAccomplished(Guid id)
        {
            var order = iOrderService.GetAll().FirstOrDefault(x => x.Id == id);
            order.Status = Data.Enums.OrderStatus.Accomplished;
            order.UpdateDate = DateTime.Now;
            order.PaymentDate = DateTime.Now;
            return iOrderService.Update(order);
        }

        [HttpDelete]
        [Route("delete-Order")]
        public bool DeleteOrder(Guid id)
        {
            var Order = iOrderService.GetAll().FirstOrDefault(c => c.Id == id);
            return iOrderService.Delete(Order);
        }

        [HttpGet]
        [Route("get-all-Order-detail")]
        public List<GProject.Data.DomainClass.OrderDetail> GetAllOrderDetail()
        {
            return iOrderService.GetAllOrderDetail();
        }

        [HttpPost]
        [Route("add-Order-detail")]
        public bool AddOrderDetail([FromBody] GProject.Data.DomainClass.OrderDetail Order)
        {
            return iOrderService.CreateOrderDetail(Order);
        }

        [HttpPost]
        [Route("update-Order-detail")]
        public bool UpdateOrderOrderDetail([FromBody] GProject.Data.DomainClass.OrderDetail Order)
        {
            return iOrderService.UpdateOrderDetail(Order);
        }

        [HttpDelete]
        [Route("delete-Order-detail")]
        public bool DeleteOrderOrderDetail(Guid id, Guid productVariation_id)
        {
            var Order = iOrderService.GetAllOrderDetail().FirstOrDefault(c => c.OrderId == id && c.ProductVariationId == productVariation_id);
            return iOrderService.DeleteOrderDetail(Order);
        }
    }
}
