using GProject.Data.DomainClass;
using System.Collections.Generic;

namespace GProject.Api.MyServices.IServices
{
    public interface ICustomerService
    {
        public Customer Login(string email, string password);
        public bool Create(Customer cv);

        public bool Update(Customer cv);

        public bool Delete(Customer cv);
        public bool ChangePassword(Customer cv);

        public List<Customer> GetAll();
    }
}
