using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class CategoryRepository:ICategoryRepository
    {
        private GProjectContext _context;
        public CategoryRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Category obj)
        {
            if (obj == null) return false;
            _context.Categories.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Category obj)
        {
            if (obj == null) return false;
            _context.Categories.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Category obj)
        {
            if (obj == null) return false;
            _context.Categories.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Category> GetAll()
        {
            return _context.Categories.OrderByDescending(c=>c.Name).ToList();
        }
    }
}
