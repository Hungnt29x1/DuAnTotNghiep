using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class ContactRepository:IContactRepository
    {
        private GProjectContext _context;
        public ContactRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Contact obj)
        {
            if (obj == null) return false;
            _context.Contacts.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Contact obj)
        {
            if (obj == null) return false;
            _context.Contacts.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Contact obj)
        {
            if (obj == null) return false;
            _context.Contacts.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Contact> GetAll()
        {
            return _context.Contacts.ToList();
        }
    }
}
