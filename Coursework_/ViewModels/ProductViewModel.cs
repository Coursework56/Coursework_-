using Coursework_.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Coursework_.ViewModels
{
    public class ProductViewModel
    {
        // Унікальний ідентифікатор товару
        public int Id { get; set; }

        // Назва товару з обов'язковими обмеженнями
        [Required(ErrorMessage = "Name is required")]
        //[Remote("IsNameUnique", "Product", ErrorMessage = "This name is already in use")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Length must be between 3 and 20 characters")]
        public string Name { get; set; }

        // Опис товару з обов'язковими обмеженнями
        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Length must be between 3 and 200 characters")]
        public string Description { get; set; }

        // Ціна товару з обов'язковими обмеженнями
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        // Шлях до фотографії товару з обов'язковими обмеженнями
        [Required(ErrorMessage = "Photo path is required")]
        public string PhotoPath { get; set; }

        // Кількість товару
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public int Amount { get; set; }

        // Ідентифікатор категорії товару з обов'язковими обмеженнями
        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryId { get; set; }

        // Назва категорії (якщо існує)
        public string? Category { get; set; }

        // Ідентифікатор виробника товару з обов'язковими обмеженнями
        [Required(ErrorMessage = "Manufacturer ID is required")]
        public int ManufacturerId { get; set; }

        // Назва виробника (якщо існує)
        public string? Manufacturer { get; set; }

        // Конструктор за замовчуванням
        public ProductViewModel() { }

        // Конструктор, який приймає об'єкт типу Product і ініціалізує відповідні властивості
        public ProductViewModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            PhotoPath = product.PhotoPath;
            ManufacturerId = product.ManufacturerId;
            CategoryId = product.CategoryId;
            Amount = product.Amount;

            // Ініціалізація назви виробника (якщо існує)
            Manufacturer = product.Manufacturer != null ? product.Manufacturer.Name : "Not found";

            // Ініціалізація назви категорії (якщо існує)
            Category = product.Category != null ? product.Category.Name : "Not found";
        }
    }
}
