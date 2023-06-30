using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IOrderDetailRepository
    {
        bool Add(OrderDetail obj);
        bool Update(OrderDetail obj);
        bool Delete(OrderDetail obj);
        List<OrderDetail> GetAll();
    }
}
