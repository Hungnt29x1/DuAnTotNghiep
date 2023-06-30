using GProject.Data.DomainClass;
using System.Collections.Generic;

namespace GProject.Api.MyServices.IServices
{
    public interface ISizeService
    {
        public bool Create(Size cv);

        public bool Update(Size cv);

        public bool Delete(Size cv);

        public List<Size> GetAll();
    }
}
