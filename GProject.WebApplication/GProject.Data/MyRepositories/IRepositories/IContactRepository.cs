using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IContactRepository
    {
        bool Add(Contact obj);
        bool Update(Contact obj);
        bool Delete(Contact obj);
        List<Contact> GetAll();
    }
}
