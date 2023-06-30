using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class SendMailRepository:ISendMailRepository
    {
        private GProjectContext _context;
        public SendMailRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(SendMail obj)
        {
            if (obj == null) return false;
            _context.SendMails.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(SendMail obj)
        {
            if (obj == null) return false;
            _context.SendMails.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(SendMail obj)
        {
            if (obj == null) return false;
            _context.SendMails.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<SendMail> GetAll()
        {
            return _context.SendMails.ToList();
        }
    }
}
