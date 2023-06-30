using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories;
using GProject.Data.MyRepositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace GProject.Api.MyServices.Services
{
    public class CategoryService: ICategoryService
    {
        private ICategoryRepository _iCategoryRepository;

        public CategoryService()
        {
            _iCategoryRepository = new CategoryRepository();
        }

        public bool Create(Category cv)
        {
            if (cv == null) return false;
            if (_iCategoryRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool Delete(Category cv)
        {
            if (cv == null) return false;
            var temp = _iCategoryRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (_iCategoryRepository.Delete(temp))
            {
                return true;
            }
            return false;
        }

        public List<Category> GetAll()
        {
            return _iCategoryRepository.GetAll();
        }

        public bool Update(Category cv)
        {
            if (cv == null) return false;
            var temp = _iCategoryRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            temp.Status = cv.Status;
            temp.SearchCount = cv.SearchCount;
            temp.Description = cv.Description;
            temp.Name = cv.Name;
            if (_iCategoryRepository.Update(temp))
            {
                return true;
            }
            return false;
        }
    }
}
