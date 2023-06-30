using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface ICustomerRepository
    {
        bool Add(Customer obj);
        bool Update(Customer obj);
        bool Delete(Customer obj);
        List<Customer> GetAll();
    }
}
