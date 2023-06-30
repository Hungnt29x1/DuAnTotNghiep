using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories;
using GProject.Data.MyRepositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace GProject.Api.MyServices.Services
{
    public class ViewHistoryService: IViewHistoryService
    {
        private IViewHistoryRepository _iViewHistoryRepository;

        public ViewHistoryService()
        {
            _iViewHistoryRepository = new ViewHistoryRepository();
        }

        public bool Create(ViewHistory cv)
        {
            if (cv == null) return false;
            if (_iViewHistoryRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool Delete(ViewHistory cv)
        {
            if (cv == null) return false;
            var temp = _iViewHistoryRepository.GetAll().FirstOrDefault(c => c.CustomerId == cv.CustomerId && c.ProductId == cv.ProductId);
            if (_iViewHistoryRepository.Delete(temp))
            {
                return true;
            }
            return false;
        }

        public List<ViewHistory> GetAll()
        {
            return _iViewHistoryRepository.GetAll();
        }

        public bool Update(ViewHistory cv)
        {
            if (cv == null) return false;
            var temp = _iViewHistoryRepository.GetAll().FirstOrDefault(c => c.CustomerId == cv.CustomerId && c.ProductId == cv.ProductId);
                        temp.DateView = cv.DateView;
            if (_iViewHistoryRepository.Update(temp))
            {
                return true;
            }
            return false;
        }
    }
}
