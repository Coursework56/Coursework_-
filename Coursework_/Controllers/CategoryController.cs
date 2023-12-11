using Coursework_.Data;
using Coursework_.Models;
using Coursework_.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coursework_.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /Category
        public IActionResult Index()
        {
            // Отримуємо всі категорії з бази даних разом з дочірніми категоріями
            var categoriesView = _dbContext.Categories
                .Include(c => c.ChildCategories)
                .Select(c => new CategoryViewModel(c))
                .ToList();

            return View(categoriesView);
        }

        // GET: /Category/Details/5
        public IActionResult Details(int id)
        {
            // Знаходимо категорію за ідентифікатором в базі даних
            var category = FindCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryView = new CategoryViewModel(category);

            var products = new List<Product>();

            // Додаємо товари поточної категорії до списку products
            if (categoryView.ProductsViews != null)
            {
                foreach (var productView in categoryView.ProductsViews)
                {
                    var product = _dbContext.Products
                        .Include(d=>d.Manufacturer)
                        .FirstOrDefault(c => c.Id == productView.Id);
                    products.Add(product);

                }
            }

            // Додаємо товари з усіх дочірніх категорій до списку products
            if (categoryView.ChildCategories != null)
            {
                foreach (var childCategory in categoryView.ChildCategories)
                {
                    category = FindCategory(childCategory.Id);
                    if (category != null && category.Products != null)
                    {
                        foreach (var productView in category.Products)
                        {

                           var product = _dbContext.Products
                                .Include(d => d.Manufacturer)
                                .FirstOrDefault(c => c.Id == productView.Id);
                                products.Add(product);
                        }
                    }
                }
            }

            // Якщо є товари, передаємо їх у ViewBag.Products
            if (products.Count > 0)
            {
                ViewBag.Products = products.Select(p=> new ProductViewModel(p)).ToList();
            }

            return View(categoryView);
        }

        // GET: /Category/Create
        public IActionResult Create(int? parentCategoryId)
        {
            // Отримуємо батьківську категорію за ідентифікатором
            var parentCategory = _dbContext.Categories.FirstOrDefault(c => c.Id == parentCategoryId);
            var categoryView = new CategoryViewModel()
            {
                ParentCategoryId = parentCategoryId,
                ParentCategory = parentCategory != null ? parentCategory.Name : null
            };

            return View(categoryView);
        }

        // POST: /Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryViewModel categoryView)
        {
            if (ModelState.IsValid)
            {
                // Перевіряємо унікальність назви категорії в базі даних
                if (_dbContext.Categories.Any(c => c.Name.ToLower() == categoryView.Name.ToLower()))
                {
                    ViewData["ErrorMessage"] = $"Категорія {categoryView.Name} вже існує";
                    return View(categoryView);
                }

                // Створюємо нову категорію та зберігаємо її в базі даних
                var category = new Category(categoryView);
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(categoryView);
        }

        // GET: /Category/Edit/5
        public IActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            // Отримуємо категорію для редагування
            var category = _dbContext.Categories
                .Include(c => c.ChildCategories)
                .FirstOrDefault(s => s.Id == Id);

            if (category == null)
                return NotFound();

            var categoryModel = new CategoryViewModel(category);
            return View(categoryModel);
        }

        // POST: /Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, CategoryViewModel categoryModel)
        {
            if (Id != categoryModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Перевіряємо унікальність назви категорії в базі даних
                if (_dbContext.Categories.Any(c => c.Name.ToLower() == categoryModel.Name.ToLower()))
                {
                    ViewData["ErrorMessage"] = $"Категорія {categoryModel.Name} вже існує";
                    return View(categoryModel);
                }

                // Оновлюємо категорію в базі даних
                var category = new Category(categoryModel);
                try
                {
                    _dbContext.Categories.Update(category);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index");
            }

            return View(categoryModel);
        }

        // GET: /Category/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Отримуємо категорію для видалення
            var category = _dbContext.Categories
                .Include(с => с.Products)
                .Include(c => c.ChildCategories)
                .FirstOrDefault(с => с.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryModel = new CategoryViewModel(category);
            return View(categoryModel);
        }

        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Видаляємо категорію з бази даних
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private bool CategoryExists(int id)
        {
            return (_dbContext.Categories?.Any(item => item.Id == id)).GetValueOrDefault();
        }

        private Category? FindCategory(int id)
        {
            // Знаходимо категорію в базі даних за ідентифікатором
            var category = _dbContext.Categories
                .Include(c => c.ChildCategories)
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id);

            return category;
        }

        // Перевірка унікальності назви категорії для валідації AJAX запитів
        public IActionResult IsNameUnique(string name)
        {
            if (_dbContext.Categories.Any(c => c.Name.ToLower() == name.ToLower()))
            {
                return Json(false);
            }
            return Json(true);
        }
    }
}
