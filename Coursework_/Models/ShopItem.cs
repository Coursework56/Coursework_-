namespace Coursework_.Models
{
    public class ShopItem
    {
        public int Id { get; set; } 

        public int ProductId { get; set; }
        public Product product { get; set; }

        public decimal Price {  get; set; }
        
        public string ShopCartId { get; set; }
    }
}
