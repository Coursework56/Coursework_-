using Coursework_.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Coursework_.ViewModels
{
    public class CategoryViewModel
    {
        // Унікальний ідентифікатор категорії
        public int Id { get; set; }

        // Назва категорії з обов'язковими обмеженнями
        [Required(ErrorMessage = "Назва необхідна")]
        [Remote("IsNameUnique", "Category", AdditionalFields = "Id", ErrorMessage = "Ця назва вже використовується")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Довжина має бути більше ніж 2 символи")]
        public string Name { get; set; }

        // Ідентифікатор батьківської категорії (якщо існує)
        public int? ParentCategoryId { get; set; }

        // Назва батьківської категорії (якщо існує)
        public string? ParentCategory { get; set; }

        // Список дочірніх категорій
        public List<CategoryViewModel>? ChildCategories { get; set; }

        // Список відображень товарів для поточної категорії
        public List<ProductViewModel>? ProductsViews { get; set; }

        // Конструктор за замовчуванням
        public CategoryViewModel() { }

        // Конструктор, який приймає об'єкт типу Category і ініціалізує відповідні властивості
        public CategoryViewModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;

            // Ініціалізація батьківської категорії, якщо вона існує
            if (category.ParentCategoryId != null)
            {
                ParentCategoryId = category.ParentCategoryId;
            }

            // Ініціалізація назви батьківської категорії, якщо вона існує
            if (category.ParentCategory != null)
            {
                ParentCategory = category.ParentCategory.Name;
            }

            // Ініціалізація дочірніх категорій, якщо вони існують
            if (category.ChildCategories != null)
            {
                ChildCategories = new List<CategoryViewModel>();
                foreach (var ChildCategory in category.ChildCategories)
                {
                    ChildCategories.Add(new CategoryViewModel(ChildCategory));
                }
            }

            // Ініціалізація товарів для поточної категорії, якщо вони існують
            if (category.Products != null)
            {
                ProductsViews = new List<ProductViewModel>();
                foreach (var product in category.Products)
                {
                    ProductsViews.Add(new ProductViewModel(product));
                }
            }
        }
    }
}
