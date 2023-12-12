using Coursework_.Models;
using Coursework_.Data.Repository;

namespace Coursework_.Data.Interfaces

{
    public interface IAllOrders
    {
        void createOrder(Order order);
    }
}
