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

        public ManufactureController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var manufactures = _dbContext.Manufacturers
                .Select(m => new ManufacturerViewModel(m))
                .ToList();

            return View(manufactures);
        }

        [HttpGet]
        public IActionResult CreateManufacturer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateManufacture(Manufacturer manufacture)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(manufacture.Name))
                {
                    ModelState.AddModelError("Name", "Поле 'Manufacture Name' є обов'язковим.");
                    return View(manufacture);
                }

                var existingManufacture = _dbContext.Manufacturers.Any(m => m.Name == manufacture.Name);
                if (existingManufacture)
                {
                    ModelState.AddModelError("Name", "Така назва виробника вже існує.");
                    return View(manufacture);
                }

                if (manufacture != null)
                {
                    _dbContext.Manufacturers.Add(manufacture);
                    _dbContext.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }

            return View(manufacture);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckManufacture(string name, int manufactureId)
        {
            var existingManufacture = _dbContext.Manufacturers
                .Any(ec => ec.Name == name && ec.Id != manufactureId);

            return Json(!existingManufacture);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckManufacturerName(string name, int manufactureId)
        {
            var existingManufacture = _dbContext.Manufacturers
                .Any(m => m.Name == name && m.Id != manufactureId);

            return Json(!existingManufacture);
        }

        public IActionResult DetailsManufacture(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacture = _dbContext.Manufacturers
                .Include(m => m.Electronics)
                .FirstOrDefault(m => m.Id == id);

            if (manufacture == null)
            {
                return NotFound();
            }

            var manufactureViewModel = new ManufacturerViewModel(manufacture);

            return View(manufactureViewModel);
        }

        public IActionResult EditManufacture(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacture = _dbContext.Manufacturers.Find(id);

            if (manufacture == null)
            {
                return NotFound();
            }

            return View(manufacture);
        }

        private bool ManufactureExists(int id)
        {
            return _dbContext.Manufacturers.Any(m => m.Id == id);
        }

        [HttpPost]
        public IActionResult EditManufacture(int id, Manufacturer manufacture)
        {
            if (id != manufacture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(manufacture);
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManufactureExists(manufacture.ManufactureId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Помилка при оновленні виробника. Будь ласка, спробуйте знову.");
                        return View(manufacture);
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(manufacture);
        }
    }
}
