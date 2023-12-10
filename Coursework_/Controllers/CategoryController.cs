using Coursework_.Models;
using Coursework_.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Coursework_.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var categories = _dbContext.Categories
                .Include(c => c.Products)
                .ToList()
                .Select(c => new CategoryViewModel(c))
                .ToList();

            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                // Конвертація з CategoryViewModel до Category
                var category = new Category
                {
                    Name = categoryViewModel.Name,
                    // Інші поля ініціалізуємо за необхідності
                };

                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(categoryViewModel);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckCategory(string name, int categoryId)
        {
            var existingCategories = _dbContext.Categories
                .Any(ec => ec.Name == name && ec.Id != categoryId);

            return Json(!existingCategories);
        }

        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _dbContext.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryViewModel = new CategoryViewModel(category);

            return View(categoryViewModel);
        }

        [HttpPost]
        public IActionResult EditCategory(int id, CategoryViewModel categoryViewModel)
        {
            if (id != categoryViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var category = _dbContext.Categories.Find(id);

                    if (category == null)
                    {
                        return NotFound();
                    }

                    // Оновлення властивостей категорії
                    category.Name = categoryViewModel.Name;
                    // Інші поля оновлюємо за необхідності

                    _dbContext.Update(category);
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Помилка при оновленні категорії. Будь ласка, спробуйте знову.");
                    return View(categoryViewModel);
                }
                return RedirectToAction("Index");
            }
            return View(categoryViewModel);
        }

        public IActionResult DetailsCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _dbContext.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryViewModel = new CategoryViewModel(category);

            return View(categoryViewModel);
        }

        private bool CategoryExists(int id)
        {
            return _dbContext.Categories.Any(c => c.Id == id);
        }
    }
}
