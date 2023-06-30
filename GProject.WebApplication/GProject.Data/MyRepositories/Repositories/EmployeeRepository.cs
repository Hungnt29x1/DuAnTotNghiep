using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private GProjectContext _context;
        public EmployeeRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Employee obj)
        {
            if (obj == null) return false;
            _context.Employees.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Employee obj)
        {
            if (obj == null) return false;
            _context.Employees.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Employee obj)
        {
            if (obj == null) return false;
            _context.Employees.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }
    }
}
