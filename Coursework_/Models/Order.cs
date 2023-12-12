using System.ComponentModel.DataAnnotations;

namespace Coursework_.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ім'я необхідне")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Довжина має бути від 3 до 20 символів")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Прізвище необхідне")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Довжина має бути від 3 до 20 символів")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Адреса необхідна")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Довжина має бути від 3 до 200 символів")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Елктронна адреса необхідна")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Довжина має бути від 3 до 30 символів")]
        public string Email { get; set; }

        public DateTime OrderTime { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
