using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class DeliverRepository:IDeliverRepository
    {
        private GProjectContext _context;
        public DeliverRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Deliver obj)
        {
            if (obj == null) return false;
            _context.Delivers.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Deliver obj)
        {
            if (obj == null) return false;
            _context.Delivers.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Deliver obj)
        {
            if (obj == null) return false;
            _context.Delivers.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Deliver> GetAll()
        {
            return _context.Delivers.ToList();
        }
    }
}
