using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface IViewHistoryRepository
    {
        bool Add(ViewHistory obj);
        bool Update(ViewHistory obj);
        bool Delete(ViewHistory obj);
        List<ViewHistory> GetAll();
    }
}
