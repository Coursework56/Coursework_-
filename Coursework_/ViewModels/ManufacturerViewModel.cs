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
        [Required(ErrorMessage = "Назва необхідна")]
        [Remote("CheckManufacturerName", "Manufacturer", AdditionalFields = "Id", ErrorMessage = "Ця назва вже використовується")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Довжина має бути більше ніж 2 символи")]
        public string Name { get; set; }

        // Опис виробника з обов'язковими обмеженнями
        [Required(ErrorMessage = "Лпис необхідний")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Довжина має бути більше ніж 2 символи")]
        public string Description { get; set; }

        // Країна виробника з обов'язковими обмеженнями
        [Required(ErrorMessage = "Країна-виробник необхідна")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Довжина має бути більше ніж 2 символи")]
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
