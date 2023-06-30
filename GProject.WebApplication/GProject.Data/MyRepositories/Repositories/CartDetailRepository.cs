using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class CartDetailRepository:ICartDetailRepository
    {
        private GProjectContext _context;
        public CartDetailRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(CartDetail obj)
        {
            if (obj == null) return false;
            _context.CartDetails.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(CartDetail obj)
        {
            if (obj == null) return false;
            _context.CartDetails.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(CartDetail obj)
        {
            try
            {
                if (obj == null) return false;
                _context.CartDetails.Update(obj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public List<CartDetail> GetAll()
        {
            return _context.CartDetails.ToList();
        }
    }
}
