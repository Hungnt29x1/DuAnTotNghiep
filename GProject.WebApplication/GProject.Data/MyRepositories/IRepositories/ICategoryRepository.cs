using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface ICategoryRepository
    {
        bool Add(Category obj);
        bool Update(Category obj);
        bool Delete(Category obj);
        List<Category> GetAll();
    }
}
