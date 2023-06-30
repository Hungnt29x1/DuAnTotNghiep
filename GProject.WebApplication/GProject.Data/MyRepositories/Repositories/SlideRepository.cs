using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class SlideRepository:ISlideRepository
    {
        private GProjectContext _context;
        public SlideRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Slide obj)
        {
            if (obj == null) return false;
            _context.Slides.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Slide obj)
        {
            if (obj == null) return false;
            _context.Slides.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Slide obj)
        {
            if (obj == null) return false;
            _context.Slides.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Slide> GetAll()
        {
            return _context.Slides.ToList();
        }
    }
}
