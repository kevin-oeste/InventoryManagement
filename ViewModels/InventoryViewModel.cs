using InventoryManagement.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models;
using System.Runtime.CompilerServices;

namespace InventoryManagement.ViewModels
{
    public class InventoryViewModel : INotifyPropertyChanged
    {
        public List<ProductViewModel> Products {
            get
            {
                return InventoryServiceProxy.Current.Items.Where(p => p != null)
                    .Select(p => new ProductViewModel(p)).ToList() 
                    ?? new List<ProductViewModel>();
            }
        }
        public void Refresh()
        {
            NotifyPropertyChanged(nameof(Products));
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
