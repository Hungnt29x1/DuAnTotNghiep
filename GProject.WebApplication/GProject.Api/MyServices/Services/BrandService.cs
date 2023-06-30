using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories;
using GProject.Data.MyRepositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace GProject.Api.MyServices.Services
{
    public class BrandService: IBrandService
    {
        private IBrandRepository _iBrandRepository;

        public BrandService()
        {
            _iBrandRepository = new BrandRepository();
        }

        public bool Create(Brand cv)
        {
            if (cv == null) return false;
            if (_iBrandRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool Delete(Brand cv)
        {
            if (cv == null) return false;
            var temp = _iBrandRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (_iBrandRepository.Delete(temp))
            {
                return true;
            }
            return false;
        }

        public List<Brand> GetAll()
        {
            return _iBrandRepository.GetAll();
        }

        public bool Update(Brand cv)
        {
            if (cv == null) return false;
            var temp = _iBrandRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            temp.Status = cv.Status;
            temp.SearchCount = cv.SearchCount;
            temp.Description = cv.Description;
            temp.Name = cv.Name;
            if (_iBrandRepository.Update(temp))
            {
                return true;
            }
            return false;
        }
    }
}
