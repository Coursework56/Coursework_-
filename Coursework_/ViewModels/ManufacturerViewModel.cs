using Coursework_.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Coursework_.ViewModels
{
    public class ManufacturerViewModel
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
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Length must be more than 2 characters")]
        public string Country { get; set; }

        // Зв'язок один до багатьох з товарами
        public List<ProductViewModel>? ProductsViews { get; set; }

        public ManufacturerViewModel() { }
        public ManufacturerViewModel(Manufacturer manufacturer)
        {
            Id = manufacturer.Id;
            Name = manufacturer.Name;
            Description = manufacturer.Description;
            Country = manufacturer.Country;

            if(manufacturer.Products != null)
            {
                ProductsViews = new List<ProductViewModel>();
                foreach(var  product in manufacturer.Products)
                {
                    ProductsViews.Add(new ProductViewModel(product));
                }
            }
        }
    }
}
