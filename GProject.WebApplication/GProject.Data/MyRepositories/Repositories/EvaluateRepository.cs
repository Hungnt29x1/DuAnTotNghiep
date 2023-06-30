using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class EvaluateRepository:IEvaluateRepository
    {
        private GProjectContext _context;
        public EvaluateRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Evaluate obj)
        {
            if (obj == null) return false;
            _context.Evaluates.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Evaluate obj)
        {
            if (obj == null) return false;
            _context.Evaluates.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Evaluate obj)
        {
            if (obj == null) return false;
            _context.Evaluates.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Evaluate> GetAll()
        {
            return _context.Evaluates.ToList();
        }
    }
}
