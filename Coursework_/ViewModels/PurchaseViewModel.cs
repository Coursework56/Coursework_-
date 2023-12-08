using Coursework_.Models;
using System.ComponentModel.DataAnnotations;

namespace Coursework_.ViewModels
{
    public class PurchaseViewModel
    {
        // Унікальний ідентифікатор покупки
        public int Id { get; set; }

        // Ім'я користувача з обов'язковими обмеженнями
        [Required(ErrorMessage = "User name is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Length must be between 3 and 20 characters")]
        public string UserName { get; set; }

        // Ідентифікатор пристрою з обов'язковими обмеженнями
        [Required(ErrorMessage = "Device ID is required")]
        public int DeviceId { get; set; }

        // Відображення товару, що був придбаний (якщо існує)
        public ProductViewModel? ProductView { get; set; }

        // Конструктор за замовчуванням
        public PurchaseViewModel() { }

        // Конструктор, який приймає об'єкт типу Purchase і ініціалізує відповідні властивості
        public PurchaseViewModel(Purchase purchase)
        {
            Id = purchase.Id;
            UserName = purchase.UserName;
            DeviceId = purchase.DeviceId;

            // Ініціалізація відображення товару, який був придбаний (якщо існує)
            if (purchase.Product != null)
            {
                ProductView = new ProductViewModel(purchase.Product);
            }
        }
    }

}
