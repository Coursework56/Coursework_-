using Coursework_.ViewModels;

namespace Coursework_.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Підкатегорії
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public List<Category>? ChildCategories { get; set; }
        // Зв'язок один до багатьох з товарами
        public List<Product>? Products { get; set; }

        public Category() { }
        public Category(CategoryViewModel categoryViewModel)
        {
            Id = categoryViewModel.Id;
            Name = categoryViewModel.Name;
            ParentCategoryId = categoryViewModel.ParentCategoryId;
        }
    }
}
