using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IProductRepository
    {
        bool Add(Product obj);
        bool Update(Product obj);
        bool Delete(Product obj);
        List<Product> GetAll();
    }
}
