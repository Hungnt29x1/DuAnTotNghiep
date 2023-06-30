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
    public class OrderDetailRepository:IOrderDetailRepository
    {
        private GProjectContext _context;
        public OrderDetailRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(OrderDetail obj)
        {
            if (obj == null) return false;
            _context.OrderDetails.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(OrderDetail obj)
        {
            if (obj == null) return false;
            _context.OrderDetails.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(OrderDetail obj)
        {
            if (obj == null) return false;
            _context.OrderDetails.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<OrderDetail> GetAll()
        {
            return _context.OrderDetails.ToList();
        }
    }
}
