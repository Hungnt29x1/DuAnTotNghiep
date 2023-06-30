using GProject.Data.DomainClass;
using System.Collections.Generic;

namespace GProject.Api.MyServices.IServices
{
    public interface IProductVariationService
    {
        public bool Create(ProductVariation cv);

        public bool Update(ProductVariation cv);

        public bool Delete(ProductVariation cv);

        public List<ProductVariation> GetAll();
    }
}
