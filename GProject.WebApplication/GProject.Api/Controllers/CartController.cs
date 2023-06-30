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
    public class CartController : ControllerBase
    {
        private ICartService iCartService;
        public CartController()
        {
            iCartService = new CartService();
        }

        [HttpGet]
        [Route("get-all-Cart")]
        public List<GProject.Data.DomainClass.Cart> GetCart()
        {
            return iCartService.GetAll();
        }

        [HttpPost]
        [Route("add-Cart")]
        public bool AddCart([FromBody] GProject.Data.DomainClass.Cart Cart)
        {
            return iCartService.Create(Cart);
        }

        [HttpPost]
        [Route("update-Cart")]
        public bool UpdateCart([FromBody] GProject.Data.DomainClass.Cart Cart)
        {
            return iCartService.Update(Cart);
        }

        [HttpDelete]
        [Route("delete-Cart")]
        public bool DeleteCart(Guid id)
        {
            var Cart = iCartService.GetAll().FirstOrDefault(c => c.Id == id);
            return iCartService.Delete(Cart);
        }

        [HttpGet]
        [Route("get-all-cart-detail")]
        public List<GProject.Data.DomainClass.CartDetail> GetAllCartDetail()
        {
            return iCartService.GetAllCartDetail();
        }

        [HttpPost]
        [Route("add-cart-detail")]
        public bool AddCartDetail([FromBody] GProject.Data.DomainClass.CartDetail Cart)
        {
            return iCartService.CreateCartDetail(Cart);
        }

        [HttpPost]
        [Route("update-cart-detail")]
        public bool UpdateCartCartDetail([FromBody] GProject.Data.DomainClass.CartDetail Cart)
        {
            return iCartService.UpdateCartDetail(Cart);
        }

        [HttpDelete]
        [Route("delete-cart-detail")]
        public bool DeleteCartCartDetail(Guid id, Guid productVariation_id)
        {
            var Cart = iCartService.GetAllCartDetail().FirstOrDefault(c => c.CartId == id && c.ProductVariationId == productVariation_id);
            return iCartService.DeleteCartDetail(Cart);
        }
    }
}
