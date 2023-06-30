using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IProductVariationRepository
    {
        bool Add(ProductVariation obj);
        bool Update(ProductVariation obj);
        bool Delete(ProductVariation obj);
        List<ProductVariation> GetAll();
    }
}
