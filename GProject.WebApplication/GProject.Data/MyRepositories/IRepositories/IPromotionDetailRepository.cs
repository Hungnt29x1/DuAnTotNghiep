using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IPromotionDetailRepository
    {
        bool Add(PromotionDetail obj);
        bool Update(PromotionDetail obj);
        bool Delete(PromotionDetail obj);
        List<PromotionDetail> GetAll();
    }
}
