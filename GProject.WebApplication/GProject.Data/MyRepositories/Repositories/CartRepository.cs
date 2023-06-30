using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class CartRepository:ICartRepository
    {
        private GProjectContext _context;
        public CartRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Cart obj)
        {
            if (obj == null) return false;
            _context.Carts.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Cart obj)
        {
            if (obj == null) return false;
            _context.Carts.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Cart obj)
        {
            if (obj == null) return false;
            _context.Carts.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Cart> GetAll()
        {
            return _context.Carts.ToList();
        }
    }
}
