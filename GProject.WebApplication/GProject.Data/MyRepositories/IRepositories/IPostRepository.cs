using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IPostsRepository
    {
        bool Add(Posts obj);
        bool Update(Posts obj);
        bool Delete(Posts obj);
        List<Posts> GetAll();
    }
}
