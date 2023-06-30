using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class PromotionRepository:IPromotionRepository
    {
        private GProjectContext _context;
        public PromotionRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Promotion obj)
        {
            if (obj == null) return false;
            _context.Promotions.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Promotion obj)
        {
            if (obj == null) return false;
            _context.Promotions.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Promotion obj)
        {
            if (obj == null) return false;
            _context.Promotions.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Promotion> GetAll()
        {
            return _context.Promotions.ToList();
        }
    }
}
