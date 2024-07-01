using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models;
using InventoryManagement.Services;

namespace InventoryManagement.ViewModels
{
    public class ProductViewModel
    {
        public override string ToString()
        {
            if(Model == null) return string.Empty;
            return $"{Model.Id} - {Model.Name} - {Model.Price:C}";
        }
        public Product? Model { get; set; }

        public string DisplayPrice
        {
            get
            {
                if(Model == null) return string.Empty;
                return $"{Model.Price:C}";
            }
        }
        public string PriceAsString
        {
            set
            {
                if(Model == null)
                {
                    return;
                }
                if(decimal.TryParse(value, out var price))
                {
                    Model.Price = price;
                }
                else
                {
                    //this is empty in the github. Why?
                    return;
                }
            }
        }
        public ProductViewModel()
        {
            Model = new Product();
        }

        public ProductViewModel(Product? model)
        {
            if(model != null) Model = model;
            else Model = new Product();
            
        }
        public void Add()
        {
            if(Model != null)
            {
                InventoryServiceProxy.Current.AddItem(Model);
            }
        }
    }
}
