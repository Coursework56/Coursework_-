using Coursework_.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Coursework_.ViewModels
{
    public class ManufacturerViewModel
    {
        // Унікальний ідентифікатор виробника
        public int Id { get; set; }

        // Назва виробника з обов'язковими обмеженнями
        [Required(ErrorMessage = "Name is required")]
        [Remote("CheckManufacturerName", "Manufacturer", ErrorMessage = "This name is already in use")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Length must be between 3 and 20 characters")]
        public string Name { get; set; }

        // Опис виробника з обов'язковими обмеженнями
        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Length must be between 3 and 200 characters")]
        public string Description { get; set; }

        // Країна виробника з обов'язковими обмеженнями
        [Required(ErrorMessage = "Country is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Length must be between 3 and 20 characters")]
        public string Country { get; set; }

        // Список відображень товарів для даного виробника
        public List<ProductViewModel>? ProductsViews { get; set; }

        // Конструктор за замовчуванням
        public ManufacturerViewModel() { }

        // Конструктор, який приймає об'єкт типу Manufacturer і ініціалізує відповідні властивості
        public ManufacturerViewModel(Manufacturer manufacturer)
        {
            Id = manufacturer.Id;
            Name = manufacturer.Name;
            Description = manufacturer.Description;
            Country = manufacturer.Country;

            // Ініціалізація товарів для даного виробника, якщо вони існують
            if (manufacturer.Products != null)
            {
                ProductsViews = new List<ProductViewModel>();
                foreach (var product in manufacturer.Products)
                {
                    ProductsViews.Add(new ProductViewModel(product));
                }
            }
        }
    }
}
