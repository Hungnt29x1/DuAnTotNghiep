using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class CustomerRepository:ICustomerRepository
    {
        private GProjectContext _context;
        public CustomerRepository()
        {
            _context = new GProjectContext();
        }

        public bool Add(Customer obj)
        {
            if (obj == null) return false;
            _context.Customers.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Customer obj)
        {
            if (obj == null) return false;
            _context.Customers.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Customer obj)
        {
            if (obj == null) return false;
            _context.Customers.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Customer> GetAll()
        {
            return _context.Customers.OrderByDescending(c =>c.UpdateDate).ToList();
        }
    }
}
