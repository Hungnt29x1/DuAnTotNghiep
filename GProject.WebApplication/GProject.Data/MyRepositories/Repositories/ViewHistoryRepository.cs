using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class ViewHistoryRepository:IViewHistoryRepository
    {
        private GProjectContext _context;
        public ViewHistoryRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(ViewHistory obj)
        {
            if (obj == null) return false;
            _context.ViewHistories.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(ViewHistory obj)
        {
            if (obj == null) return false;
            _context.ViewHistories.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(ViewHistory obj)
        {
            if (obj == null) return false;
            _context.ViewHistories.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<ViewHistory> GetAll()
        {
            return _context.ViewHistories.ToList();
        }
    }
}
