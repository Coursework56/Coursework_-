using Coursework_.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coursework_.ViewModels;
using Coursework_.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Volt.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var productsViews = _dbContext.Products
                .Include(p=>p.Manufacturer)
                .Include(p=>p.Category)
                .Select(p=>new ProductViewModel(p))
                .ToList();


            return View(productsViews);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductViewModel productViewModel)
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
                    ManufacturerId = productViewModel.ManufacturerId
                };

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            PopulateDropdowns();
            return View(productViewModel);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckProductName(string name)
        {
            var existingProduct = _dbContext.Products
                .Any(ec => ec.Name == name);

            return Json(!existingProduct);
        }

        private void PopulateDropdowns()
        {
            var categories = _dbContext.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            var manufacturers = _dbContext.Manufacturers.ToList();
            ViewBag.Manufacturers = new SelectList(manufacturers, "Id", "Name");
        }

        public IActionResult DetailsProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = new ProductViewModel(product);
            return View(productViewModel);
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = new ProductViewModel(product);
            PopulateDropdowns();
            return View(productViewModel);
        }

        [HttpPost]
        public IActionResult EditProduct(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.Id == productViewModel.Id);

                if (product == null)
                {
                    return NotFound();
                }

                product.Name = productViewModel.Name;
                product.Description = productViewModel.Description;
                product.Price = productViewModel.Price;
                product.PhotoPath = productViewModel.PhotoPath;
                product.Amount = productViewModel.Amount;
                product.CategoryId = productViewModel.CategoryId;
                product.ManufacturerId = productViewModel.ManufacturerId;

                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            PopulateDropdowns();
            return View(productViewModel);
        }

        private bool ProductExists(int id)
        {
            return _dbContext.Products.Any(p => p.Id == id);
        }

        public async Task<ActionResult> Buy(int? id)
        {
            if (!id.HasValue)
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id.Value);

                if (product != null)
                {
                    return NotFound();
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult Buy(int productId)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            if (product.Amount > 0)
            { 
                product.Amount -= 1;

               
                var purchase = new Purchase
                {
                    Id = product.Id,
                    
                };

                _dbContext.Purchases.Add(purchase);
                _dbContext.SaveChanges();

               
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("ProductUnavailable", "Error");
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            var product = _dbContext.Products
                .Include(p=>p.Manufacturer)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id==id);
            var productView = new ProductViewModel(product);
            return View(productView);
        }


        [HttpPost,ActionName("DeleteProduct")]
        public IActionResult DeleteProductConfirmed(int id)
        {
            var product = _dbContext.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
