using Coursework_.ViewModels;

namespace Coursework_.Models
{
    public class Manufacturer
    {
        // Унікальний ідентифікатор виробника
        public int Id { get; set; }

        // Назва виробника
        public string Name { get; set; }

        // Опис виробника
        public string Description { get; set; }

        // Країна виробника
        public string Country { get; set; }

        // Список товарів даного виробника (якщо існує)
        public List<Product>? Products { get; set; }

        // Конструктор за замовчуванням
        public Manufacturer() { }

        // Конструктор, який приймає об'єкт типу ManufacturerViewModel і ініціалізує відповідні властивості
        public Manufacturer(ManufacturerViewModel manufacturerViewModel)
        {
            Id = manufacturerViewModel.Id;
            Name = manufacturerViewModel.Name;
            Description = manufacturerViewModel.Description;
            Country = manufacturerViewModel.Country;
        }
    }
}
