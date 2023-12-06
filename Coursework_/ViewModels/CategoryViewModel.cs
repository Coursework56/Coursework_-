using Coursework_.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Coursework_.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Required]
        [Remote("IsNameUnique", "Product", ErrorMessage = "This name is already in use")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Length must be more than 2 characters")]
        public string Name { get; set; }

        // Підкатегорії
        public int? ParentCategoryId { get; set; }
        public string? ParentCategory { get; set; }
        // Зв'язок один до багатьох з товарами
        public List<ProductViewModel>? ProductsViews { get; set; }

        public CategoryViewModel() { }
        public CategoryViewModel(Category category) 
        {
            Id = category.Id;
            Name = category.Name;
            if(category.ParentCategoryId != null)
            {
                ParentCategoryId = category.ParentCategoryId;
            }
            if (category.ParentCategory != null)
            {
                ParentCategory = category.ParentCategory.Name;
            }
            if(category.Products != null)
            {
                ProductsViews = new List<ProductViewModel>();
                foreach(var product in category.Products)
                {
                    ProductsViews.Add(new ProductViewModel(product));
                }
            }
        } 
    }
}
