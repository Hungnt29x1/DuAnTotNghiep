using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface ICartDetailRepository
    {
        bool Add(CartDetail obj);
        bool Update(CartDetail obj);
        bool Delete(CartDetail obj);
        List<CartDetail> GetAll();
    }
}
