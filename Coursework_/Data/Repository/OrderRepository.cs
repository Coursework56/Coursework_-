using Coursework_.Data.Interfaces;
using Coursework_.Models;

namespace Coursework_.Data.Repository
{
    public class OrderRepository : IAllOrders
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ShopCart _cart;

        public OrderRepository(ApplicationDbContext dbContext, ShopCart cart)
        {
            this._dbContext = dbContext;
            this._cart = cart;
        }

            public void createOrder(Order order)
            {
                order.OrderTime = DateTime.Now;
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges(); // Зберегти зміни для отримання згенерованого OrderId

                var items = _cart.listShopItems;
                foreach (var item in items)
                {
                    var orderDetail = new OrderDetail()
                    {
                        ProductId = item.product.Id,
                        OrderId = order.Id, // Замініть це на правильний OrderId, отриманий після збереження змін
                        Price = item.product.Price
                    };

                    _dbContext.OrderDetails.Add(orderDetail);
                }

                _dbContext.SaveChanges(); // Зберегти зміни після додавання всіх OrderDetails
            }
        }
    }
