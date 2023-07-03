using GProject.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.DomainClass
{
    public class Order
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

        [Column(TypeName = "decimal(20, 0)")]
        public decimal? ShippingFee { get; set; }
        [Column(TypeName = "decimal(20, 0)")]
        public decimal TotalMoney { get; set; }
        public OrderStatus Status { get; set; }
        public string? Description { get; set; }
        public string? ReasonForChange { get; set; }
        public string? HistoryLogChange { get; set; }
        public Employee? EmployeeId_Nagavition { get; set; }
        public Customer? CustomerId_Nagavition { get; set; }
        public Deliver? DeliverId_Nagavition { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
