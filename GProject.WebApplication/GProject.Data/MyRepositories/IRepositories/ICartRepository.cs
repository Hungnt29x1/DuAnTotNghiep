using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface ICartRepository
    {
        bool Add(Cart obj);
        bool Update(Cart obj);
        bool Delete(Cart obj);
        List<Cart> GetAll();
    }
}
