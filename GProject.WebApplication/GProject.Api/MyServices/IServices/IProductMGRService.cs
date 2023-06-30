using GProject.Data.DomainClass;
using System.Collections.Generic;

namespace GProject.Api.MyServices.IServices
{
    public interface IProductMGRService
    {
        public bool Create(Product cv);

        public bool Update(Product cv);

        public bool Delete(Product cv);

        public List<Product> GetAll();
    }
}
