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
        [Required(ErrorMessage = "Назва необхідна")]
        [Remote("CheckProductName", "Product", ErrorMessage = "Ця назва вже використовується")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Довжина має бути більше ніж 2 символи")]
        public string Name { get; set; }

        // Опис товару з обов'язковими обмеженнями
        [Required(ErrorMessage = "Опис необхідний")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Довжина має бути більше ніж 2 символи")]
        public string Description { get; set; }

        // Ціна товару з обов'язковими обмеженнями
        [Required(ErrorMessage = "Цін анеобхідна")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ціна має бути більше 0")]
        public decimal Price { get; set; }

        // Шлях до фотографії товару з обов'язковими обмеженнями
        [Required(ErrorMessage = "Посилання для вото необхідне")]
        public string PhotoPath { get; set; }

        // Кількість товару
        [Range(0, int.MaxValue, ErrorMessage = "Кількість має бути більше 0 або більше")]
        public int Amount { get; set; }

        // Ідентифікатор категорії товару з обов'язковими обмеженнями
        [Required(ErrorMessage = "Категорія має бути вибрана")]
        public int CategoryId { get; set; }

        // Назва категорії (якщо існує)
        public string? Category { get; set; }

        // Ідентифікатор виробника товару з обов'язковими обмеженнями
        [Required(ErrorMessage = "Категорія має бути вибрана")]
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
