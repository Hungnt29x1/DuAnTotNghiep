using GProject.Data.DomainClass;
using System.Collections.Generic;

namespace GProject.Api.MyServices.IServices
{
    public interface IEmployeeService
    {
        public Employee Login(string email, string password);
        public bool Create(Employee cv);

        public bool Update(Employee cv);

        public bool Delete(Employee cv);
        public bool ChangePassword(Employee cv);

        public List<Employee> GetAll();
    }
}
