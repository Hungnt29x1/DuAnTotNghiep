using GProject.Data.DomainClass;
using System.Collections.Generic;

namespace GProject.Api.MyServices.IServices
{
    public interface ICartService
    {
        public bool Create(Cart cv);

        public bool Update(Cart cv);

        public bool Delete(Cart cv);

        public List<Cart> GetAll();

        public bool CreateCartDetail(CartDetail cv);

        public bool DeleteCartDetail(CartDetail cv);

        public List<CartDetail> GetAllCartDetail();

        public bool UpdateCartDetail(CartDetail cv);
    }
}
