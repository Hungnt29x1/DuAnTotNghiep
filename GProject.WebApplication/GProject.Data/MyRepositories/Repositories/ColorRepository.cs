using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class ColorRepository:IColorRepository
    {
        private GProjectContext _context;
        public ColorRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Color obj)
        {
            if (obj == null) return false;
            _context.Colors.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Color obj)
        {
            if (obj == null) return false;
            _context.Colors.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Color obj)
        {
            if (obj == null) return false;
            _context.Colors.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Color> GetAll()
        {
            return _context.Colors.OrderByDescending(c => c.CreateOrUpdate).ToList();
        }
    }
}
