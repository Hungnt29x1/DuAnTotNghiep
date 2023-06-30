using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class PromotionDetailRepository:IPromotionDetailRepository
    {
        private GProjectContext _context;
        public PromotionDetailRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(PromotionDetail obj)
        {
            if (obj == null) return false;
            _context.PromotionDetails.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(PromotionDetail obj)
        {
            if (obj == null) return false;
            _context.PromotionDetails.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(PromotionDetail obj)
        {
            if (obj == null) return false;
            _context.PromotionDetails.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<PromotionDetail> GetAll()
        {
            return _context.PromotionDetails.ToList();
        }
    }
}
