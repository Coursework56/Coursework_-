using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using Coursework_.Data;
using Coursework_.Models;
using Coursework_.ViewModels;

namespace Coursework_.Controllers
{
    public class ManufacturerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ManufacturerController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var manufacturers = _dbContext.Manufacturers
                .Select(m => new ManufacturerViewModel(m))
                .ToList();

            return View(manufacturers);
        }

        [HttpGet]
        public IActionResult CreateManufacturer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateManufacturer(Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(manufacturer.Name))
                {
                    ModelState.AddModelError("Name", "Поле 'Manufacturer Name' є обов'язковим.");
                    return View(manufacturer);
                }

                var existingManufacturer = _dbContext.Manufacturers.Any(m => m.Name == manufacturer.Name);
                if (existingManufacturer)
                {
                    ModelState.AddModelError("Name", "Така назва виробника вже існує.");
                    return View(manufacturer);
                }

                if (manufacturer != null)
                {
                    _dbContext.Manufacturers.Add(manufacturer);
                    _dbContext.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }

            return View(manufacturer);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckManufacturerName(string name, int id)
        {
            var existingManufacturer = _dbContext.Manufacturers
                .Any(m => m.Name == name && m.Id != id);

            return Json(!existingManufacturer);
        }

        public IActionResult DetailsManufacturer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = _dbContext.Manufacturers
                .Include(m => m.Products)
                .FirstOrDefault(m => m.Id == id);

            if (manufacturer == null)
            {
                return NotFound();
            }

            var manufacturerViewModel = new ManufacturerViewModel(manufacturer);

            return View(manufacturerViewModel);
        }

        public IActionResult EditManufacturer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = _dbContext.Manufacturers.Find(id);

            if (manufacturer == null)
            {
                return NotFound();
            }

            var viewModel = new ManufacturerViewModel
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                Country = manufacturer.Country,
                Description = manufacturer.Description
            };

            return View(viewModel);
        }

        public IActionResult DeleteManufacturer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = _dbContext.Manufacturers.Find(id);

            if (manufacturer == null)
            {
                return NotFound();
            }

            var manufacturerViewModel = new ManufacturerViewModel
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name ?? string.Empty,
                Country = manufacturer.Country ?? string.Empty,
                Description = manufacturer.Description ?? string.Empty
            };

            if (manufacturer.Products != null && manufacturer.Products.Count > 0)
            {
                return BadRequest("Помилка при видалені компанії!!!");
            }
            else
            {
                // Return the view to confirm the deletion of the manufacturer
                return DeleteConfirmed(id.Value);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var manufacturer = _dbContext.Manufacturers.Find(id);

            if (manufacturer == null)
            {
                return NotFound();
            }

            _dbContext.Manufacturers.Remove(manufacturer);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home"); // Перенаправлення на домашню сторінку
        }

        private bool ManufacturerExists(int id)
        {
            return _dbContext.Manufacturers.Any(m => m.Id == id);
        }

        [HttpPost]
        public IActionResult EditManufacturer(int id, Manufacturer manufacturer)
        {
            if (id != manufacturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(manufacturer);
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManufacturerExists(manufacturer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Помилка при оновленні виробника. Будь ласка, спробуйте знову.");
                        return View(manufacturer);
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(manufacturer);
        }
    }
}
