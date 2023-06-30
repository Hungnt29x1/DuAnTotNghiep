using GProject.Data.DomainClass;
using System.Collections.Generic;

namespace GProject.Api.MyServices.IServices
{
    public interface ICategoryService
    {
        public bool Create(Category cv);

        public bool Update(Category cv);

        public bool Delete(Category cv);

        public List<Category> GetAll();
    }
}
