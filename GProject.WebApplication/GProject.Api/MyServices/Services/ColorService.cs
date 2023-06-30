using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories;
using GProject.Data.MyRepositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace GProject.Api.MyServices.Services
{
    public class ColorService: IColorService
    {
        private IColorRepository _iColorRepository;

        public ColorService()
        {
            _iColorRepository = new ColorRepository();
        }

        public bool Create(Color cv)
        {
            if (cv == null) return false;
            cv.CreateOrUpdate = DateTime.Now;
            if (_iColorRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool Delete(Color cv)
        {
            if (cv == null) return false;
            var temp = _iColorRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (_iColorRepository.Delete(temp))
            {
                return true;
            }
            return false;
        }

        public List<Color> GetAll()
        {
            return _iColorRepository.GetAll();
        }

        public bool Update(Color cv)
        {
            if (cv == null) return false;
            var temp = _iColorRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
                        temp.HEXCode = cv.HEXCode;
                        temp.Name = cv.Name;
            temp.Image = cv.Image;
            temp.CreateOrUpdate = DateTime.Now;
            temp.Status = cv.Status;
            if (_iColorRepository.Update(temp))
            {
                return true;
            }
            return false;
        }
    }
}
