using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class PostsRepository:IPostsRepository
    {
        private GProjectContext _context;
        public PostsRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(Posts obj)
        {
            if (obj == null) return false;
            _context.Posts.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Posts obj)
        {
            if (obj == null) return false;
            _context.Posts.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Posts obj)
        {
            if (obj == null) return false;
            _context.Posts.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<Posts> GetAll()
        {
            return _context.Posts.ToList();
        }
    }
}
