using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class OrderRepository:IOrderRepository
    {
        private GProjectContext _context;
        public OrderRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Order obj)
        {
            if (obj == null) return false;
            _context.Orders.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Order obj)
        {
            if (obj == null) return false;
            _context.Orders.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Order obj)
        {
            if (obj == null) return false;
            _context.Orders.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Order> GetAll()
        {
            return _context.Orders.ToList();
        }
    }
}
