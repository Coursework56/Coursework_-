using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Coursework_.Data;

namespace Coursework_.Models
{
    public class ShopCart
    {
        public readonly ApplicationDbContext _dbContext;
        public ShopCart(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
            this.listShopItems = new List<ShopItem>();

        }
        
        [Key]
        public string ShopCartId { get; set; }

        public List<ShopItem> listShopItems { get; set; }

        public static ShopCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<ApplicationDbContext>();
            string shopCartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", shopCartId);

            return new ShopCart(context) { ShopCartId = shopCartId };
        }

        public void AddToCart(Product product)
        {
            this._dbContext.ShopItems.Add(new ShopItem
            {
                ShopCartId = ShopCartId,
                product = product,
                ProductId = product.Id,
                Price = product.Price,
            });

            _dbContext.SaveChanges();
        }

        public List<ShopItem> GetShopItems()
        {
            return _dbContext.ShopItems.Where(c => c.ShopCartId == ShopCartId)
                .Include(s => s.product)
                .ToList();
        }

        public void ClearCart()
        {
            listShopItems.Clear();
       
        }

        public bool HasItemsInCart(ISession session)
        {
            string shopCartId = session.GetString("CartId");

            if (shopCartId == null)
            {
                return false; // якщо ID корзини в сес≥њ в≥дсутн≥й, то товар≥в в корзин≥ немаЇ
            }

            return _dbContext.ShopItems.Any(c => c.ShopCartId == shopCartId);
        }

        public List<int> GetProductIdsInCart()
        {
            return _dbContext.ShopItems
                .Where(c => c.ShopCartId == ShopCartId)
                .Select(s => s.product.Id)
                .ToList();
        }

        public void RemoveFromCart(Product product)
        {
            var shopCartId = ShopCartId;
            var itemToRemove = _dbContext.ShopItems.FirstOrDefault(i => i.product.Id == product.Id && i.ShopCartId == shopCartId);

            if (itemToRemove != null)
            {
                _dbContext.ShopItems.Remove(itemToRemove);
                _dbContext.SaveChanges();
            }
        }

    }

}
