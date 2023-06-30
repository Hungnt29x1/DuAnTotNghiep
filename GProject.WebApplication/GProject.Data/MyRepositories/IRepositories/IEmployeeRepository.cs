using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IEmployeeRepository
    {
        bool Add(Employee obj);
        bool Update(Employee obj);
        bool Delete(Employee obj);
        List<Employee> GetAll();
    }
}
