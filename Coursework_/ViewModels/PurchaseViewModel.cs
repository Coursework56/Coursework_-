using Coursework_.Models;
using System.ComponentModel.DataAnnotations;

namespace Coursework_.ViewModels
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Length must be more than 2 characters")]
        public string UserName { get; set; }
        [Required]
        public int DeviceId { get; set; }
        public ProductViewModel? ProductView { get; set; }

        public PurchaseViewModel() { }
        public PurchaseViewModel(Purchase purchase) 
        {
            Id = purchase.Id;
            UserName = purchase.UserName;
            DeviceId = purchase.DeviceId;

            if(purchase.Product != null) 
            {
                ProductView = new ProductViewModel(purchase.Product);
            }
        }

    }
}
