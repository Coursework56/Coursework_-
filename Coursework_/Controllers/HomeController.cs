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

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
{
    var products = _dbContext.Products
        .Include(p => p.Category)
        .Include(p => p.Manufacturer)
        .Select(p => new ProductViewModel(p))
        .ToList();

    return View(products);
}

        public IActionResult Options()
        {
            return View();
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
