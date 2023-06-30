using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories;
using GProject.Data.MyRepositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace GProject.Api.MyServices.Services
{
    public class EmployeeService:IEmployeeService
    {
        //Đây là ví dụ thì chỉ có 4 chức năng, nhưng trong bài toán thật sẽ có thể có các chứng năng, tìm kiếm chức vụ, lọc chức vụ...., Tại đây sẽ gọi đến rất nhiều repo khác nhau để phục vụ bài toán.
        private IEmployeeRepository _iEmployeeRepository;

        public EmployeeService()
        {
            _iEmployeeRepository = new EmployeeRepository();
        }

        public Employee Login(string email, string pass)
        {
            var temp = _iEmployeeRepository.GetAll().FirstOrDefault(c => c.Email == email && c.Password == pass);
            if (temp != null)
            {
                return temp;
            }
            else
            {
                return null;
            }
        }
        public bool Create(Employee cv)
        {
            if (cv == null) return false;
            if (_iEmployeeRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool Delete(Employee cv)
        {
            if (cv == null) return false;
            var temp = _iEmployeeRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (temp == null) return false;
            if (_iEmployeeRepository.Delete(temp))
            {
                return true;
            }
            return false;
        }

        public List<Employee> GetAll()
        {
            return _iEmployeeRepository.GetAll();
        }

        public bool Update(Employee cv)
        {
            if (cv == null) return false;
            var temp = _iEmployeeRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (temp == null) return false;
            temp.Name = cv.Name;
            temp.Password = cv.Password;
            temp.PersonalId = cv.PersonalId;
            temp.DateOfBirth = cv.DateOfBirth;
            temp.DateOfJoin = cv.DateOfJoin;
            temp.Sex = cv.Sex;
            temp.PhoneNumber = cv.PhoneNumber;
            temp.Address = cv.Address;
            temp.Address = cv.Address;
            temp.Description = cv.Description;
            temp.Image = cv.Image;
            temp.Status = cv.Status;
            if (_iEmployeeRepository.Update(temp))
            {
                return true;
            }
            return false;
        }

        public bool ChangePassword(Employee cv)
        {
            if (cv == null) return false;
            var temp = _iEmployeeRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (temp == null) return false;
            temp.Password = cv.Password;
            if (_iEmployeeRepository.Update(temp))
            {
                return true;
            }
            return false;
        }
    }
}
