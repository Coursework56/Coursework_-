using Coursework_.Data;
using Coursework_.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coursework_.Controllers
{
    public class ProductСontroller : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductСontroller(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var categoriesView = _dbContext.Categories
                .Include(c=>c.ChildCategories)
                .Select(c=> new CategoryViewModel(c))
                .ToList(); 
            return View(categoriesView);
        }

        public IActionResult Details(int id)
        {
            var category = _dbContext.Categories
                .Include(c=>c.ChildCategories)
                .Include(c=>c.Products)
                .FirstOrDefault(c=>c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryView = new CategoryViewModel(category);
            return View();
        }
    }
}
