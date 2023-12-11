using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using Coursework_.Data;
using Coursework_.Models;
using Coursework_.ViewModels;

namespace Coursework_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ShopCart _cart;

        public HomeController(ApplicationDbContext dbContext, ShopCart cart)
        {
            _dbContext = dbContext;
            _cart = cart;
        }

        public IActionResult Index()
        {
            var products = _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    PhotoPath = p.PhotoPath,
                    CategoryId = p.CategoryId,
                    ManufacturerId = p.ManufacturerId
                })
                .ToList();

            return View(products);
        }

        public ViewResult ShopCart()
        {
            var items = _cart.GetShopItems();
            _cart.listShopItems = items;

            var obj = new ShopCartViewModel
            {
                shopCart = _cart
            };

            return View(obj);
        }

        public RedirectToActionResult AddToCart(int id)
        {
            var item = _dbContext.Products.FirstOrDefault(i => i.Id == id);
            if (item != null && item.Amount > 0) // Перевірка наявності товару
            {
                _cart.AddToCart(item); // Додаємо товар до кошика

                // Віднімаємо одиницю від кількості товару
                item.Amount--;

                // Оновлюємо зміни в базі даних (або вашому сховищі)
                _dbContext.SaveChanges(); // Зберігаємо зміни у базі даних
            }
            return RedirectToAction("Index");
        }

        public IActionResult Options()
        {
            return View();
            //commit
        }

        [HttpGet]
        public IActionResult Search(string searchString)
        {
            var products = _dbContext.Products
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(searchString) || p.Name.Contains(searchString))
                .ToList();

            var productViewModels = products.Select(p => new ProductViewModel(p)).ToList();

            return View("Index", productViewModels);
        }

        [HttpGet]
        public IActionResult Buy(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var item = _dbContext.Products.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        public IActionResult Buy(Purchase purchase)
        {
            purchase.DateTime = DateTime.Now;
            _dbContext.Purchases.Add(purchase);
            _dbContext.SaveChanges();

            _cart.ClearCart();

            TempData["PurchaseSuccess"] = "Покупку оформлено успішно!";

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
