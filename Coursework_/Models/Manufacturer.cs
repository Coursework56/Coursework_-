using Coursework_.ViewModels;

namespace Coursework_.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }

        // Зв'язок один до багатьох з товарами
        public List<Product>? Products { get; set; }

        public Manufacturer() { }
        public Manufacturer(ManufacturerViewModel manufacturerViewModel)
        {
            Id = manufacturerViewModel.Id;
            Name = manufacturerViewModel.Name;
            Description = manufacturerViewModel.Description;
            Country = manufacturerViewModel.Country;
        }
    }
}
