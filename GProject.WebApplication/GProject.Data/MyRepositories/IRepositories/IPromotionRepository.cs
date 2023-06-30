using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IPromotionRepository
    {
        bool Add(Promotion obj);
        bool Update(Promotion obj);
        bool Delete(Promotion obj);
        List<Promotion> GetAll();
    }
}
