using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.MyRepositories.Repositories
{
    public class DeliveryAddressRepository : IDeliveryAddressRepository
    {
        private GProjectContext _context;
        public DeliveryAddressRepository() { 
            _context = new GProjectContext();
        }
        public bool Add(DeliveryAddress obj)
        {
            if (obj == null) return false;
            _context.DeliveryAddresses.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(DeliveryAddress obj)
        {
            if (obj == null) return false;
            _context.DeliveryAddresses.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public List<DeliveryAddress> GetAll()
        {
            return _context.DeliveryAddresses.ToList();
        }

        public bool Update(DeliveryAddress obj)
        {
            if (obj == null) return false;
            _context.DeliveryAddresses.Update(obj);
            _context.SaveChanges();
            return true;
        }
    }
}
