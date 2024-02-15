using EasyNetQ.Topology;
using Google.Apis.Util;
using GProject.Data.Context;
using GProject.Data.DomainClass;
using GProject.WebApplication.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PagedList.Core;

namespace GProject.WebApplication.Controllers
{
    public class PostsController : Controller
    {
        private readonly GProjectContext _context;

        public PostsController(GProjectContext context)
        {
            _context = context;
        }

        [Route("/tin-tuc/{id}.html", Name = "TinChiTiet")]
        public IActionResult DetailCustomer(Guid id)
        {
            var catname = _context.Categories.ToList();
            var userName = _context.Employees.ToList();

            var tindang = _context.Posts.AsNoTracking().SingleOrDefault(x => x.Id == id);
            if (tindang == null)
            {
                return RedirectToAction("Index");
            }
            var lsBaivietlienquan = _context.Posts
                .AsNoTracking()
                .Where(x => x.Published == true && x.Id != id)
                .Take(3)
                .OrderByDescending(x => x.CreatedDate).ToList();
            if(tindang.Views == null)
            {
                tindang.Views = 0;
            }
            tindang.Views += 1;
            _context.Update(tindang);
            _context.SaveChanges();

            var lsReviews = _context.Reviews
                .Include(c => c.PostId_Navigation)
                .Include(c => c.CustomerId_Navigation)
                .Where(c => c.PostId == tindang.Id).ToList();
            ViewBag.Review = lsReviews;

            ViewBag.Baivietlienquan = lsBaivietlienquan;
            ViewBag.CatName = catname;
            ViewBag.NameAuthor = userName;

            return View(tindang);
        }

        public IActionResult ListBlog(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;

            var singleTinViewPlus = _context.Posts.AsNoTracking().OrderBy(c => c.Views).Take(1).ToList();
            var catname = _context.Categories.ToList();

            var lsTinDangs = _context.Posts
                .AsNoTracking()
                .OrderBy(x => x.UpdatedDate);
            PagedList<Posts> models = new PagedList<Posts>(lsTinDangs, pageNumber, pageSize);


            ViewBag.CurrentPage = pageNumber;
            ViewBag.SttShowData = pageSize;
            ViewBag.DataTinHot = singleTinViewPlus;
            ViewBag.CatName = catname;
            return View(models);
        }


        public IActionResult Index(int? page)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                return RedirectToAction("AccessDenied", "Account");

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsTinDangs = _context.Posts
                .AsNoTracking()
            .OrderBy(x => x.UpdatedDate);
            PagedList<Posts> models = new PagedList<Posts>(lsTinDangs, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinDang = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tinDang == null)
            {
                return NotFound();
            }

            return View(tinDang);
        }

        // GET: Admin/AdminTinDangs/Create
        public IActionResult Create()
        {
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Admin/AdminTinDangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Posts tinDang, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            var employee = HttpContext.Session.GetObjectFromJson<Employee>("userLogin");
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name");

            //Xu ly Thumb
            if (fThumb != null)
            {
                string extension = Path.GetExtension(fThumb.FileName);
                string imageName = Commons.SEOUrl(tinDang.Title) + extension;
                tinDang.Thumb = await Commons.UploadFile(fThumb, @"news", imageName.ToLower());
            }
            if (string.IsNullOrEmpty(tinDang.Thumb)) tinDang.Thumb = "default.jpg";
            tinDang.Alias = Commons.SEOUrl(tinDang.Title);
            tinDang.CreatedDate = DateTime.Now;
            tinDang.UpdatedDate = DateTime.Now;
            tinDang.EmployeeId = employee.Id;
            tinDang.Author = employee.Name;
            tinDang.PostId = 0;
            tinDang.Views = 0;

            _context.Add(tinDang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name");
            var tinDang = await _context.Posts.FindAsync(id);
            if (tinDang == null)
            {
                return NotFound();
            }
            return View(tinDang);
        }

        // POST: Admin/AdminTinDangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, Posts tinDang, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != tinDang.Id)
            {
                return NotFound();
            }

            try
            {
                ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name");
                var employee = HttpContext.Session.GetObjectFromJson<Employee>("userLogin");
                //Xu ly Thumb
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imageName = Commons.SEOUrl(tinDang.Title) + extension;
                    tinDang.Thumb = await Commons.UploadFile(fThumb, @"news", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(tinDang.Thumb)) tinDang.Thumb = "default.jpg";
                tinDang.Alias = Commons.SEOUrl(tinDang.Title);
                tinDang.UpdatedDate = DateTime.Now;
                tinDang.EmployeeId = employee.Id;
                _context.Update(tinDang);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinDang = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tinDang == null)
            {
                return NotFound();
            }

            return View(tinDang);
        }

        // POST: Admin/AdminTinDangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tinDang = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(tinDang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TinDangExists(Guid id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }


    }
}
