using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IEvaluateRepository
    {
        bool Add(Evaluate obj);
        bool Update(Evaluate obj);
        bool Delete(Evaluate obj);
        List<Evaluate> GetAll();
    }
}
