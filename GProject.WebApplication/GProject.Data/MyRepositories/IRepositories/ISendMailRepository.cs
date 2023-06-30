using GProject.Data.DomainClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public interface ISendMailRepository
    {
        bool Add(SendMail obj);
        bool Update(SendMail obj);
        bool Delete(SendMail obj);
        List<SendMail> GetAll();
    }
}
