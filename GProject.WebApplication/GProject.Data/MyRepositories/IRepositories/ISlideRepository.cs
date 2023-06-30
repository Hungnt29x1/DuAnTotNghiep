using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface ISlideRepository
    {
        bool Add(Slide obj);
        bool Update(Slide obj);
        bool Delete(Slide obj);
        List<Slide> GetAll();
    }
}
