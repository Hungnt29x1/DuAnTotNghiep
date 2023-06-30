using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class BrandRepository:IBrandRepository
    {
        private GProjectContext _context;
        public BrandRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Brand obj)
        {
            if (obj == null) return false;
            _context.Brands.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Brand obj)
        {
            if (obj == null) return false;
            _context.Brands.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Brand obj)
        {
            if (obj == null) return false;
            _context.Brands.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Brand> GetAll()
        {
            return _context.Brands.ToList();
        }
    }
}
