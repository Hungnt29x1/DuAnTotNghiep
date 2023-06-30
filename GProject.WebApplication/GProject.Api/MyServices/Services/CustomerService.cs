using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories;
using GProject.Data.MyRepositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace GProject.Api.MyServices.Services
{
    public class CustomerService : ICustomerService
    {
        //Đây là ví dụ thì chỉ có 4 chức năng, nhưng trong bài toán thật sẽ có thể có các chứng năng, tìm kiếm chức vụ, lọc chức vụ...., Tại đây sẽ gọi đến rất nhiều repo khác nhau để phục vụ bài toán.
        private ICustomerRepository _iCustomerRepository;

        public CustomerService()
        {
            _iCustomerRepository = new CustomerRepository();
        }
        public Customer Login(string email, string pass)
        {
            var temp = _iCustomerRepository.GetAll().FirstOrDefault(c => c.Email == email && c.Password == pass);
            if (temp != null)
            {
                return temp;
            }
            else
            {
                return null;
            }
        }
        public bool Create(Customer cv)
        {
            if (cv == null) return false;
            if (_iCustomerRepository.Add(cv))
            {
                return true;
            }
            return false;
        }

        public bool Delete(Customer cv)
        {
            if (cv == null) return false;
            var temp = _iCustomerRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (temp != null)
            {
                _iCustomerRepository.Delete(temp);
                return true;
            }
            return false;
        }

        public List<Customer> GetAll()
        {
            return _iCustomerRepository.GetAll();
        }

        public bool Update(Customer cv)
        {
            if (cv == null) return false;
            var temp = _iCustomerRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id || c.CustomerId == cv.CustomerId);
            if (temp == null) return false;
            temp.Name = cv.Name;
            temp.Password = cv.Password;
            temp.PhoneNumber = cv.PhoneNumber;
            temp.Sex = cv.Sex;
            temp.DateOfBirth = cv.DateOfBirth;
            temp.Description = cv.Description;
            temp.Address = cv.Address;
            temp.Image = cv.Image;
            temp.Status = cv.Status;
            if (_iCustomerRepository.Update(temp))
            {
                return true;
            }
            return false;
        }

        public bool ChangePassword(Customer cv)
        {
            if (cv == null) return false;
            var temp = _iCustomerRepository.GetAll().FirstOrDefault(c => c.Id == cv.Id);
            if (temp == null) return false;
            temp.Password = cv.Password;
            if (_iCustomerRepository.Update(temp))
            {
                return true;
            }
            return false;
        }
    }
}
