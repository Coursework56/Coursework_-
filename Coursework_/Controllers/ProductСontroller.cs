using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Coursework_.Data;
using Coursework_.Models;
using Coursework_.ViewModels;

namespace Volt.Controllers
{
    public class ElectronicController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ElectronicController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Метод Index змінено для використання ViewModel
        public IActionResult Index()
        {
            var products = _dbContext.Products.ToList()
                .Select(product => new ProductViewModel(product))
                .ToList();

            return View(products);
        }

        // Метод CreateElectronic переписано для використання ViewModel
        [HttpGet]
        public IActionResult CreateElectronic()
        {
            var electronicClass = _dbContext.ElectronicClasses.ToList();
            ViewBag.ElectronicClasses = new SelectList(electronicClass, "ElectronicClassId", "Name");

            var company = _dbContext.Companies.ToList();
            ViewBag.Companies = new SelectList(company, "CompanyId", "CompanyName");

            return View();
        }

        [HttpPost]
        public IActionResult CreateElectronic(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = productViewModel.Name,
                    Description = productViewModel.Description,
                    Price = productViewModel.Price,
                    PhotoPath = productViewModel.PhotoPath,
                    Amount = productViewModel.Amount,
                    CategoryId = productViewModel.CategoryId,
                    ManufacturerId = productViewModel.ManufacturerId,
                    // інші властивості ініціалізуємо за потреби
                };

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            PopulateDropdowns();
            return View(productViewModel);
        }

        // Інші методи контролера тут...

        private void PopulateDropdowns()
        {
            var electronicClass = _dbContext.ElectronicClasses.ToList();
            ViewBag.ElectronicClasses = new SelectList(electronicClass, "ElectronicClassId", "Name");

            var company = _dbContext.Companies.ToList();
            ViewBag.Companies = new SelectList(company, "CompanyId", "CompanyName");
        }

        // Метод EditElectronic переписано для використання ViewModel
        [HttpGet]
        public IActionResult EditElectronic(int id)
        {
            var product = _dbContext.Products.FirstOrDefault(e => e.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = new ProductViewModel(product);

            PopulateDropdowns();

            return View(productViewModel);
        }

        [HttpPost]
        public IActionResult EditElectronic(int id, ProductViewModel updatedProductViewModel)
        {
            if (id != updatedProductViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = _dbContext.Products.FirstOrDefault(e => e.Id == id);

                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    // Оновлення властивостей товару
                    existingProduct.Name = updatedProductViewModel.Name;
                    existingProduct.Description = updatedProductViewModel.Description;
                    existingProduct.Price = updatedProductViewModel.Price;
                    existingProduct.PhotoPath = updatedProductViewModel.PhotoPath;
                    existingProduct.Amount = updatedProductViewModel.Amount;
                    existingProduct.CategoryId = updatedProductViewModel.CategoryId;
                    existingProduct.ManufacturerId = updatedProductViewModel.ManufacturerId;
                    // інші властивості оновлюємо за необхідності

                    _dbContext.Update(existingProduct);
                    _dbContext.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(updatedProductViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Помилка при оновленні товару. Будь ласка, спробуйте знову.");
                        PopulateDropdowns();
                        return View(updatedProductViewModel);
                    }
                }
            }

            PopulateDropdowns();
            return View(updatedProductViewModel);
        }

        // Метод DetailsElectronic переписано для використання ViewModel
        public IActionResult DetailsElectronic(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _dbContext.Products.FirstOrDefault(e => e.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = new ProductViewModel(product);

            return View(productViewModel);
        }

        // Метод DeleteElectronic переписано для використання ViewModel
        [HttpGet]
        public IActionResult DeleteElectronic(int id)
        {
            var product = _dbContext.Products.FirstOrDefault(e => e.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        private bool ProductExists(int id)
        {
            return _dbContext.Products.Any(e => e.Id == id);
        }

        // Метод Buy переписано для використання ViewModel
        [HttpGet]
        public IActionResult Buy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _dbContext.Products.FirstOrDefault(e => e.Id == id.Value);

            if (product == null)
            {
                return NotFound();
            }

            return View(new ProductViewModel(product));
        }

        [HttpPost]
        public string Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;
            _dbContext.Purchases.Add(purchase);
            _dbContext.SaveChanges();

            return "Дякуємо, " + purchase.PurchaseName + ", за купівлю!";
        }
    }
}
