using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using Coursework_.Data;
using Coursework_.Models;
using Coursework_.ViewModels;
using Microsoft.AspNetCore.Http;
using Coursework_.Data.Interfaces;

namespace Coursework_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ShopCart _cart;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAllOrders _allOrders;

        public HomeController(ApplicationDbContext dbContext, ShopCart cart, IHttpContextAccessor httpContextAccessor, IAllOrders allOrders)
        {
            _dbContext = dbContext;
            _cart = cart;
            _httpContextAccessor = httpContextAccessor;
            _allOrders = allOrders;
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
                    ManufacturerId = p.ManufacturerId,
                    Amount= p.Amount
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

        [HttpGet]
        [HttpGet]
        public IActionResult Search(string searchString)
        {
            var products = _dbContext.Products
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString)) // Consider searching in Description as well
                .ToList();

            var productViewModels = products.Select(p => new ProductViewModel(p)).ToList();

            return View("Index", productViewModels); // Pass the search results to the Index view
        }

        public RedirectToActionResult AddToCart(int id)
        {
            var item = _dbContext.Products.FirstOrDefault(i => i.Id == id);
            if (item != null && item.Amount > 0) // Перевірка наявності товару перед додаванням до кошика
            {
                _cart.AddToCart(item);
                item.Amount--; // Віднімаємо одиницю від кількості товару в базі даних
                _dbContext.SaveChanges(); // Оновлюємо зміни в базі даних
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var productToRemove = _dbContext.Products.FirstOrDefault(p => p.Id == productId);

            if (productToRemove != null)
            {
                int originalAmount = productToRemove.Amount; // Збереження оригінальної кількості товару

                _cart.RemoveFromCart(productToRemove);
                // Видалення товару з корзини

                // Додавання одиниці товару назад до бази даних
                if (originalAmount >= 0) // Перевірка, щоб уникнути від'ємної кількості
                {
                    productToRemove.Amount = originalAmount + 1;
                    _dbContext.SaveChanges();
                }

                return RedirectToAction("ShopCart");
            }

            return NotFound(); // Або повернення іншого результату в разі невдачі
        }


        public IActionResult Options()
        {
            return View();
        }



        [HttpGet]
        public IActionResult Buy()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Buy(Order order)
        {
            _cart.listShopItems = _cart.GetShopItems();
            if(_cart.listShopItems == null || _cart.listShopItems.Count == 0)
            {
                ModelState.AddModelError("", "Корзина порожня! Якщо ви хочете оформити покупку, виберіть щось!");
            }

            if(ModelState.IsValid)
            {
                _dbContext.SaveChanges();
                _httpContextAccessor.HttpContext.Session.Clear();
                _cart.ClearCart();
                _allOrders.createOrder(order);
                return RedirectToAction("Complete");
                
            }

           

            return View(order);
        }

        public IActionResult Complete()
        {
            ViewBag.Message = "Замовлення успішно оформлено!";
            return View();
        }


        

        public IActionResult Clear()
        {
            _cart.ClearCart(); // Очищення кошика

            _httpContextAccessor.HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }

        //commit

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
