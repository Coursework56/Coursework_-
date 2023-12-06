using Coursework_.ViewModels;

namespace Coursework_.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PhotoPath { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set;}

        public Product() { }
        public Product(ProductViewModel productViewModel)
        {
            Id = productViewModel.Id;
            Name = productViewModel.Name;
            Description = productViewModel.Description;
            Price = productViewModel.Price;
            PhotoPath = productViewModel.PhotoPath;
            CategoryId = productViewModel.CategoryId;
            ManufacturerId = productViewModel.ManufacturerId;
        }
    }
}
