using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IDeliveryAddressRepository
    {
        bool Add(DeliveryAddress obj);
        bool Update(DeliveryAddress obj);
        bool Delete(DeliveryAddress obj);
        List<DeliveryAddress> GetAll();
    }
}
