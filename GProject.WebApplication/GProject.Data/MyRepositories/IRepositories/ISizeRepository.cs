using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface ISizeRepository
    {
        bool Add(Size obj);
        bool Update(Size obj);
        bool Delete(Size obj);
        List<Size> GetAll();
    }
}
