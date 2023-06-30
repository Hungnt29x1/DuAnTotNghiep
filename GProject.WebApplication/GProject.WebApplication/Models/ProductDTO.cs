using GProject.Data.DomainClass;
using GProject.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace GProject.WebApplication.Models
{
    public class ProductDTO
    {
        public Product Product { get; set; }

        public Category Category { get; set; }

        public Color Color { get; set; }

        public Size Size { get; set; }
        public Brand Brand { get; set; }

        public List<ProductVariation> ProductVariations { get; set; }
        public Promotion Promotion { get; set; }

        public PromotionDetail PromotionDetail { get; set; }
    }

    public class OrderDTO
    {
        public Guid? Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? DeliverId { get; set; }
        public string OrderId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public string ShippingFullName { get; set; }
        public string ShippingCountry { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingDistrict { get; set; }
        public string ShippingTown { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingPhone { get; set; }
        public string ShippingEmail { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalMoney { get; set; }
        public OrderStatus Status { get; set; }
        public string? Description { get; set; }
        public Employee? EmployeeId_Nagavition { get; set; }
        public Customer? CustomerId_Nagavition { get; set; }
        public Deliver? DeliverId_Nagavition { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
        public List<Order>? OrderList { get; set; }
        public List<ShowOrderDto>? ShowOrderDtoList { get; set; }
    }
    public class ProdOrder
    {
        public string cartId { get; set; }
        public string prodVariationId { get; set; }
        public string productId { get; set; }
        public float price { get; set; }
        public int size { get; set; }
        public int color { get; set; }
        public int quantity { get; set; }
        public float totalMoneyItem { get; set; }
    }
}
