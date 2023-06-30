using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GProject.Data.MyRepositories.IRepositories
{
    public class ProductVariationRepository:IProductVariationRepository
    {
        private GProjectContext _context;
        public ProductVariationRepository()
        {
            _context = new GProjectContext();
        }
        public bool Add(ProductVariation obj)
        {
            if (obj == null) return false;
            _context.ProductVariations.Add(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(ProductVariation obj)
        {
            if (obj == null) return false;
            _context.ProductVariations.Remove(obj);
            _context.SaveChanges();
            return true;
        }

        public bool Update(ProductVariation obj)
        {
            if (obj == null) return false;
            _context.ProductVariations.Update(obj);
            _context.SaveChanges();
            return true;
        }

        public List<ProductVariation> GetAll()
        {
            return _context.ProductVariations.ToList();
        }
    }
}
