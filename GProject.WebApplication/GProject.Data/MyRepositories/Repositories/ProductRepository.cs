using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class ProductRepository:IProductRepository
    {
        private GProjectContext _context;
        public ProductRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Product obj)
        {
            if (obj == null) return false;
            _context.Products.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Product obj)
        {
            if (obj == null) return false;
            _context.Products.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Product obj)
        {
            if (obj == null) return false;
            _context.Products.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Product> GetAll()
        {
            return _context.Products.OrderByDescending(c=>c.CreateDate).ToList();
        }
    }
}
