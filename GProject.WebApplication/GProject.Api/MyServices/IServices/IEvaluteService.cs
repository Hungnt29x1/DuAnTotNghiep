using GProject.Data.DomainClass;
using System.Collections.Generic;

namespace GProject.Api.MyServices.IServices
{
    public interface IEvaluateService
    {
        public bool Create(Evaluate cv);

        public bool Update(Evaluate cv);

        public bool Delete(Evaluate cv);

        public List<Evaluate> GetAll();
    }
}
