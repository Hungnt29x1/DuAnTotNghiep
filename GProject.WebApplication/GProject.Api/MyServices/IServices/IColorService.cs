using GProject.Data.DomainClass;
using System.Collections.Generic;

namespace GProject.Api.MyServices.IServices
{
    public interface IColorService
    {
        public bool Create(Color cv);

        public bool Update(Color cv);

        public bool Delete(Color cv);

        public List<Color> GetAll();
    }
}
