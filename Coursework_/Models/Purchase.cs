using Coursework_.ViewModels;

namespace Coursework_.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int DeviceId { get; set; }
        public DateTime DateTime { get; set; }
        public Product? Product { get; set; }

        public Purchase() { }
        public Purchase(PurchaseViewModel purchaseViewModel)
        {
            Id = purchaseViewModel.Id;
            UserName = purchaseViewModel.UserName;
            DeviceId = purchaseViewModel.DeviceId;
            DateTime = DateTime.Now;

            if(purchaseViewModel.ProductView != null)
            {
                Product = new Product(purchaseViewModel.ProductView);
            }
        }

    }
}
