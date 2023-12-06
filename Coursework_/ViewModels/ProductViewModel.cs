using Coursework_.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Coursework_.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [Remote("IsNameUnique", "Product", ErrorMessage = "This name is already in use")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Length must be more than 2 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Length must be more than 2 characters")]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        [Required]
        public string PhotoPath { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string? Category { get; set; }
        [Required]
        public int ManufacturerId { get; set; }
        public string? Manufacturer { get; set; }

        public ProductViewModel() { }
        public ProductViewModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            PhotoPath = product.PhotoPath;
            ManufacturerId = product.ManufacturerId;
            CategoryId = product.CategoryId;

            Manufacturer = product.Manufacturer != null ? product.Manufacturer.Name : "Not found";
            Category = product.Category != null ? product.Category.Name : "Not found";

        }
    }
}
