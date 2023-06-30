using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories;
using GProject.Data.MyRepositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace GProject.Api.MyServices.Services
{
    public class EvaluateService: IEvaluateService
    {
        private IEvaluateRepository _iEvaluateRepository;

        public EvaluateService()
        {
            _iEvaluateRepository = new EvaluateRepository();
        }

        public bool Create(Evaluate cv)
        {
            if (cv == null) return false;
            if (_iEvaluateRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool Delete(Evaluate cv)
        {
            if (cv == null) return false;
            var temp = _iEvaluateRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (_iEvaluateRepository.Delete(temp))
            {
                return true;
            }
            return false;
        }

        public List<Evaluate> GetAll()
        {
            return _iEvaluateRepository.GetAll();
        }

        public bool Update(Evaluate cv)
        {
            if (cv == null) return false;
            var temp = _iEvaluateRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
                        temp.Comment = cv.Comment;
                        temp.Rating = cv.Rating;
            temp.UpdateDate = cv.UpdateDate;
            if (_iEvaluateRepository.Update(temp))
            {
                return true;
            }
            return false;
        }
    }
}
