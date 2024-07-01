using InventoryManagement.Models;
using InventoryManagement.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        public ShopViewModel() { 
            InventoryQuery = string.Empty;
        }
        private string inventoryQuery;
        public string InventoryQuery
        {
            set {
                inventoryQuery = value;
                NotifyPropertyChanged();
            }
            get { return inventoryQuery; }
        }
        public List<ProductViewModel> Products
        {
            get
            {
                return InventoryServiceProxy.Current.Items.Where(p => p != null)
                    .Where(p => p?.Name?.ToUpper()?.Contains(InventoryQuery.ToUpper()) ?? false)
                    .Select(p=> new ProductViewModel(p)).ToList()
                    ?? new List<ProductViewModel>();
            }
        }
        public List<ProductViewModel> ProductsInCart
        {
            get
            {
                return ShoppingCartServiceProxy.Current?.Cart?.Contents?.Where(p => p != null)
                    .Where(p => p?.Name?.ToUpper()?.Contains(InventoryQuery.ToUpper()) ?? false)
                    .Select(p => new ProductViewModel(p)).ToList()
                    ?? new List<ProductViewModel>();
            }
        }
        private ProductViewModel? productToBuy;
        public ProductViewModel? ProductToBuy
        {
            get => productToBuy;
            set
            {
                productToBuy = value;
                if (productToBuy != null && productToBuy.Model == null)
                {
                    productToBuy.Model = new Models.Product();
                }
                else if (productToBuy != null && productToBuy.Model != null)
                {
                    productToBuy.Model = new Models.Product(productToBuy.Model);
                }
            }
        }
        public ShoppingCart Cart
        {
            get
            {
                return ShoppingCartServiceProxy.Current.Cart;
            }
        }

        public void Refresh()
        {
            InventoryQuery = string.Empty;
            NotifyPropertyChanged(nameof(Products));
        }

        public void Search()
        {
            NotifyPropertyChanged(nameof(Products));
        }
        public void PlaceInCart()
        {
            if (productToBuy?.Model == null)
            {
                return;
            }
            productToBuy.Model.Amount = 1;
            ShoppingCartServiceProxy.Current.AddProduct(ProductToBuy.Model, ProductToBuy.Model.Amount);
            
            ProductToBuy = null;
            NotifyPropertyChanged(nameof(ProductsInCart));
            NotifyPropertyChanged(nameof (Products));
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
