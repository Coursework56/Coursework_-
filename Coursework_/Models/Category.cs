using Coursework_.ViewModels;

namespace Coursework_.Models
{
    public class Category
    {
        // Унікальний ідентифікатор категорії
        public int Id { get; set; }

        // Назва категорії
        public string Name { get; set; }

        // Ідентифікатор батьківської категорії (якщо існує)
        public int? ParentCategoryId { get; set; }

        // Об'єкт батьківської категорії (якщо існує)
        public Category? ParentCategory { get; set; }

        // Список дочірніх категорій (якщо існує)
        public List<Category>? ChildCategories { get; set; }

        // Список товарів в даній категорії (якщо існує)
        public List<Product>? Products { get; set; }

        // Конструктор за замовчуванням
        public Category() { }

        // Конструктор, який приймає об'єкт типу CategoryViewModel і ініціалізує відповідні властивості
        public Category(CategoryViewModel categoryViewModel)
        {
            Id = categoryViewModel.Id;
            Name = categoryViewModel.Name;
            ParentCategoryId = categoryViewModel.ParentCategoryId;
        }
    }
}
