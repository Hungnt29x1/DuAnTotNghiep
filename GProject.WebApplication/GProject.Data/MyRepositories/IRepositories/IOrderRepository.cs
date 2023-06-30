using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IOrderRepository
    {
        bool Add(Order obj);
        bool Update(Order obj);
        bool Delete(Order obj);
        List<Order> GetAll();
    }
}
