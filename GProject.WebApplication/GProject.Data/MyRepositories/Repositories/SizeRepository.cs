using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class SizeRepository:ISizeRepository
    {
        private GProjectContext _context;
        public SizeRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Size obj)
        {
            if (obj == null) return false;
            _context.Sizes.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Size obj)
        {
            if (obj == null) return false;
            _context.Sizes.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Size obj)
        {
            if (obj == null) return false;
            _context.Sizes.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Size> GetAll()
        {
            return _context.Sizes.ToList();
        }
    }
}
