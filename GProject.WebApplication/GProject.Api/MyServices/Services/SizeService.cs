using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories;
using GProject.Data.MyRepositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace GProject.Api.MyServices.Services
{
    public class SizeService: ISizeService
    {
        private ISizeRepository _iSizeRepository;

        public SizeService()
        {
            _iSizeRepository = new SizeRepository();
        }

        public bool Create(Size cv)
        {
            if (cv == null) return false;
            if (_iSizeRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool Delete(Size cv)
        {
            if (cv == null) return false;
            var temp = _iSizeRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (_iSizeRepository.Delete(temp))
            {
                return true;
            }
            return false;
        }

        public List<Size> GetAll()
        {
            return _iSizeRepository.GetAll();
        }

        public bool Update(Size cv)
        {
            if (cv == null) return false;
            var temp = _iSizeRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
                        temp.Code = cv.Code;
                        temp.Name = cv.Name;
            temp.Status = cv.Status;
            if (_iSizeRepository.Update(temp))
            {
                return true;
            }
            return false;
        }
    }
}
