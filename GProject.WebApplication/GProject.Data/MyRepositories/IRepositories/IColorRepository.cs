using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IColorRepository
    {
        bool Add(Color obj);
        bool Update(Color obj);
        bool Delete(Color obj);
        List<Color> GetAll();
    }
}
