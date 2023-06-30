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
    public class VoucherRepository : IVoucherRepository
    {
        private readonly GProjectContext _context;
        public VoucherRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Voucher obj)
        {
            if (obj == null) return false;
            _context.Vouchers.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Voucher obj)
        {
            if (obj == null) return false;
            _context.Vouchers.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Voucher> GetAll()
        {
            return _context.Vouchers.ToList();
        }

        public bool Update(Voucher obj)
        {
            if (obj == null) return false;
            _context.Vouchers.Update(obj);
            _context.SaveChanges();
            return true;
        }
    }
}
