using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IVoucherRepository
    {
        bool Add(Voucher obj);
        bool Update(Voucher obj);
        bool Delete(Voucher obj);
        List<Voucher> GetAll();
    }
}
