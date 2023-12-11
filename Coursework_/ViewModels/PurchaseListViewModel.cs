using Coursework_.Models;
using System.Collections.Generic;

namespace Coursework_.ViewModels
{
    public class PurchaseListViewModel
    {
        public List<PurchaseViewModel> Purchases { get; set; }

        public PurchaseListViewModel()
        {
            Purchases = new List<PurchaseViewModel>();
        }

        public PurchaseListViewModel(List<Purchase> purchases)
        {
            Purchases = new List<PurchaseViewModel>();

            foreach (var purchase in purchases)
            {
                Purchases.Add(new PurchaseViewModel(purchase));
            }
        }
    }
}