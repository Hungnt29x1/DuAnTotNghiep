using GProject.Data.DomainClass;
using GProject.WebApplication.Models.DeliveryAddressAndShippingFee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GProject.WebApplication.Models
{
	public class CartDTO
	{
		public Cart Cart { get; set; }
		public Brand Brand { get; set; }
		public CartDetail CartDetail { get; set; }
		public Product Product { get; set; }
		public ProductVariation ProductVariation { get; set; }
		public Color Color { get; set; }
		public Size Size { get; set; }
	}
}