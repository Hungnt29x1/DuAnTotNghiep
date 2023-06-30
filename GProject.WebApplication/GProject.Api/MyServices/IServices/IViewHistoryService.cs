using GProject.Data.DomainClass;
using System.Collections.Generic;

namespace GProject.Api.MyServices.IServices
{
    public interface IViewHistoryService
    {
        public bool Create(ViewHistory cv);

        public bool Update(ViewHistory cv);

        public bool Delete(ViewHistory cv);

        public List<ViewHistory> GetAll();
    }
}
