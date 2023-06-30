using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IBrandRepository
    {
        bool Add(Brand obj);
        bool Update(Brand obj);
        bool Delete(Brand obj);
        List<Brand> GetAll();
    }
}
