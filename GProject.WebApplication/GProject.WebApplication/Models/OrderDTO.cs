using GProject.Data.DomainClass;
using GProject.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace GProject.WebApplication.Models
{
    public class OrderDto
    {
        public Order Order { get; set; }
        public Brand Brand { get; set; }
        public OrderDetail OrderDetail { get; set; }
        public Product Product { get; set; }
        public ProductVariation ProductVariation { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }
        public List<Order> Orders { get; set; }
    }

    public class OrderViewModel
    {
        public Guid? Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? DeliverId { get; set; }
        public string OrderId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public string ShippingFullName { get; set; }
        public string ShippingCountry { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingDistrict { get; set; }
        public string ShippingTown { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingPhone { get; set; }
        public string ShippingEmail { get; set; }
        public int? VoucherId { get; set; }
        public float? DiscountRate { get; set; }
        public decimal? ShippingFee { get; set; }
        public decimal TotalMoney { get; set; }
        public OrderStatus Status { get; set; }
        public string? Description { get; set; }
        public string? OrderDetailJson { get; set; }
        public string? ReasonForChange { get; set; }
        public string? HistoryLogChange { get; set; }
    }
}
