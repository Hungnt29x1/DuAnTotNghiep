using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.WebApplication.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GProject.WebApplication.Controllers
{
    public class ReviewController : Controller
    {
        private readonly GProjectContext _context;

        public ReviewController(GProjectContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Reviews review)
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
            if (customer == null)
            {
                return Json(new { result = 0 });
            }
            if(review.Comment == null || review.Comment == "" || String.IsNullOrEmpty(review.Comment))
            {
                return Json(new { result = 2 });
            }
            review.CreateDate = DateTime.Now;
            _context.Reviews.Add(review);
            _context.SaveChanges();
            return Json(new { result = 1 });
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
            if (customer == null)
            {
                return Json(new { result = 0 });
            }
            var reviewToDelete = _context.Reviews.FirstOrDefault(c => c.Id.ToString().ToLower() == id.ToString().ToLower() && c.AccountID == customer.Id);

            if (reviewToDelete == null) {
                return Json(new { result = 2 });
            }
            _context.Reviews.Remove(reviewToDelete);
            _context.SaveChanges();
            return Json(new { result = 1 });
        }
    }
}
