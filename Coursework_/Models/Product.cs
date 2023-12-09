using Coursework_.ViewModels;

namespace Coursework_.Models
{
    public class Product
    {
        // Унікальний ідентифікатор товару
        public int Id { get; set; }

        // Назва товару
        public string Name { get; set; }

        // Опис товару
        public string Description { get; set; }

        // Ціна товару
        public decimal Price { get; set; }

        // Шлях до фотографії товару
        public string PhotoPath { get; set; }

        // Ідентифікатор категорії товару
        public int CategoryId { get; set; }

        // Кількість товару
        public int Amount { get; set; }

        // Категорія товару (якщо існує)
        public Category? Category { get; set; }

        // Ідентифікатор виробника товару
        public int ManufacturerId { get; set; }

        // Виробник товару (якщо існує)
        public Manufacturer? Manufacturer { get; set; }

        // Конструктор за замовчуванням
        public Product() { }

        // Конструктор, який приймає об'єкт типу ProductViewModel і ініціалізує відповідні властивості
        public Product(ProductViewModel productViewModel)
        {
            Id = productViewModel.Id;
            Name = productViewModel.Name;
            Description = productViewModel.Description;
            Price = productViewModel.Price;
            PhotoPath = productViewModel.PhotoPath;
            CategoryId = productViewModel.CategoryId;
            ManufacturerId = productViewModel.ManufacturerId;
            Amount = productViewModel.Amount;
        }
    }
}
