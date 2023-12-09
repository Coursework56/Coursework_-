using Coursework_.ViewModels;

namespace Coursework_.Models
{
    public class Purchase
    {
        // Унікальний ідентифікатор покупки
        public int Id { get; set; }

        // Ім'я користувача
        public string UserName { get; set; }

        // Ідентифікатор пристрою
        public int DeviceId { get; set; }

        // Дата та час здійснення покупки
        public DateTime DateTime { get; set; }

        // Товар, що був придбаний (якщо існує)
        public Product? Product { get; set; }

        // Конструктор за замовчуванням
        public Purchase() { }

        // Конструктор, який приймає об'єкт типу PurchaseViewModel і ініціалізує відповідні властивості
        public Purchase(PurchaseViewModel purchaseViewModel)
        {
            Id = purchaseViewModel.Id;
            UserName = purchaseViewModel.UserName;
            DeviceId = purchaseViewModel.DeviceId;
            DateTime = DateTime.Now;

            // Ініціалізація товару, що був придбаний (якщо існує)
            if (purchaseViewModel.ProductView != null)
            {
                Product = new Product(purchaseViewModel.ProductView);
            }
        }
    }
}
