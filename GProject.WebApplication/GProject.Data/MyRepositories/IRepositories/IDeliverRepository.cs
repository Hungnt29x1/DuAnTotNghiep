using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IDeliverRepository
    {
        bool Add(Deliver obj);
        bool Update(Deliver obj);
        bool Delete(Deliver obj);
        List<Deliver> GetAll();
    }
}
