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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ApplicationDbContext dbContext, ShopCart cart, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _cart = cart;
            _httpContextAccessor = httpContextAccessor;
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
            _cart.AddToCart(item);
            return RedirectToAction("Index");
        }

        public IActionResult Options()
        {
            return View();
        }



        [HttpGet]
        public IActionResult Buy(int? id)
        {
            var itemsInCart = _cart.GetShopItems();

            if (id.HasValue)
            {
                var item = _dbContext.Products.FirstOrDefault(i => i.Id == id.Value);

                if (item != null && item.Amount > 0) // Перевірка наявності товару
                {
                    // Додаємо товар до кошика

                    // Віднімаємо одиницю від кількості товару
                    item.Amount--;

                    // Оновлюємо зміни в базі даних (або вашому сховищі)
                    _dbContext.SaveChanges(); // Зберігаємо зміни у базі даних
                }
                else
                {
                    TempData["EmptyCart"] = "Корзина порожня";
                    return RedirectToAction("Index");
                }
            }

            if (!itemsInCart.Any())
            {
                TempData["EmptyCart"] = "Корзина порожня";
                return RedirectToAction("Index");
            }

            var purchases = itemsInCart.Select(item => new PurchaseViewModel
            {
                UserName = "", // Ініціалізуйте інші необхідні властивості покупки
                DeviceId = id ?? 0 // Використовуємо DeviceId отриманий з параметра методу або за замовчуванням 0
            }).ToList();

            var purchaseListViewModel = new PurchaseListViewModel
            {
                Purchases = purchases
            };

            return View(purchaseListViewModel);
        }

        [HttpPost]
        public IActionResult Buy(PurchaseListViewModel purchaseListViewModel)
        {
            if (!_cart.HasItemsInCart(_httpContextAccessor.HttpContext.Session))
            {
                TempData["EmptyCart"] = "Корзина порожня";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid || purchaseListViewModel.Purchases == null || !purchaseListViewModel.Purchases.Any())
            {
                // Повернути представлення з PurchaseListViewModel у випадку недійсних даних або порожнього списку покупок
                return View(purchaseListViewModel);
            }

            // Логіка для збереження покупок в базі даних
            foreach (var purchaseViewModel in purchaseListViewModel.Purchases)
            {
                var purchase = new Purchase
                {
                    UserName = purchaseViewModel.UserName,
                    DeviceId = purchaseViewModel.DeviceId,
                    DateTime = DateTime.Now,
                };

                _dbContext.Purchases.Add(purchase);
            }

            // Збереження всіх покупок в базі даних
            _dbContext.SaveChanges();
            _httpContextAccessor.HttpContext.Session.Clear();
            _cart.ClearCart();

            TempData["PurchaseSuccess"] = "Покупку оформлено успішно!";

            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            _cart.ClearCart(); // Очищення кошика

            _httpContextAccessor.HttpContext.Session.Clear();

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
