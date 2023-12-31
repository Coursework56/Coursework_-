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
        public IActionResult CheckProductName(string name, int? id)
        {
            var existingManufacturer = _dbContext.Products
                .Any(m => m.Name == name && m.Id != id);

            return Json(!existingManufacturer);
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


        [HttpGet]
        public async Task<ActionResult> BuyProduct(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var product = await _dbContext.Products
                .Include(p => p.Manufacturer)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(e => e.Id == Id);

            if (product == null)
            {
                return NotFound();
            }

            var purchaseViewModel = new PurchaseViewModel
            {
                ProductView = new ProductViewModel(product)
                // You might need to set other properties of PurchaseViewModel as required
            };

            return View(purchaseViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> BuyProduct(Purchase purchase)
        {
            var electronic = await _dbContext.Products.FirstOrDefaultAsync(e => e.Id == purchase.Id);

            if (electronic != null && electronic.Amount > 0)
            {
                purchase.DateTime = DateTime.Now;

                purchase.Id = 0;

                _dbContext.Purchases.Add(purchase);

                electronic.Amount--; // Deduct the quantity
                await _dbContext.SaveChangesAsync(); // Save changes asynchronously

                return RedirectToAction("Complete", "Home"); // Redirect to Complete action in Home controller
            }

            return NotFound(); // Or any other appropriate action if the product is not available
        }

        [HttpPost]
        public async Task<IActionResult> CancelPurchase(int? purchaseId)
        {
            if (purchaseId == null)
            {
                return NotFound();
            }

            var purchase = await _dbContext.Purchases.FirstOrDefaultAsync(p => p.Id == purchaseId);

            if (purchase != null)
            {
                var product = purchase.Product; // ��������� ��������, ���'������� � ��������

                if (product != null)
                {
                    product.Amount++; // �������� ������� ������
                    _dbContext.Purchases.Remove(purchase); // �������� ����� ��� �������
                    await _dbContext.SaveChangesAsync(); // �������� ���� ����������

                    return RedirectToAction("Index", "Home"); // ������������� �� ������ ������ ��� ����-���� ��������� ������
                }
            }

            return NotFound(); // ��� ����-��� ���� �������� ������, ���� ������� ��� ����� �� ��������
        }


        [HttpGet]
        public IActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
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
