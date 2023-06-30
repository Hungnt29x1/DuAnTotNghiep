using GProject.Data.DomainClass;
using System.Collections.Generic;

namespace GProject.Api.MyServices.IServices
{
    public interface IBrandService
    {
        public bool Create(Brand cv);

        public bool Update(Brand cv);

        public bool Delete(Brand cv);

        public List<Brand> GetAll();
    }
}
