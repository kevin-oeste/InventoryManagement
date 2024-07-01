using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models;
using System.Collections.ObjectModel;

namespace InventoryManagement.Services
{

    public class InventoryServiceProxy
    {
        private List<Product> inventory;
        private InventoryServiceProxy()
        {
            inventory = new List<Product>
            {
                new Product{Name = "Apple", Description = "Red fruit.", Price = (decimal)0.99, Amount = 20},
                new Product{Name = "Bannana", Description = "Yellow fruit.", Price = (decimal)0.79, Amount = 40},
                new Product{Name = "Egg", Description = "Chicken egg.", Price = (decimal)0.21, Amount = 120}
            };
        }

        private static InventoryServiceProxy? instance;
        private static object? instanceLock = new object();
        public static InventoryServiceProxy Current
        {

            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new InventoryServiceProxy();
                    }
                }
                return instance;
            }
        }

        public ReadOnlyCollection<Product>? Items
        {
            get
            {
                return inventory?.AsReadOnly();
            }
        }
        //=================== Inventory Management =====================

        public Product AddItem(Product newItem)
        {
            //check if item is already in system
            if (inventory.Any(item => item.Id.Equals(newItem.Id)))
            {
                //if already in system, add its stock value to existing stock
                inventory.First(item => item.Id.Equals(newItem.Id)).Amount += newItem.Amount;
            }
            else
            {
                inventory.Add(newItem);
            }
            return newItem;
        }

        public void UpdateItem(Product item1, string newName, string newDescription, decimal newPrice, int newStock)
        {
            item1.Name = newName;
            item1.Description = newDescription;
            item1.Price = newPrice;
            item1.Amount = newStock;
        }

        public void RemoveItem(Product trash)
        {
            var removalItem = inventory.FirstOrDefault(item => item.Id.Equals(trash.Id));
            if (removalItem != null)
            {
                inventory.Remove(removalItem);
            }
        }
        private Product GetProductById(Guid id)
        {
            return inventory.FirstOrDefault(item => item.Id.Equals(id));
        }

        public bool ContainsItem(Guid id)
        {
            return GetProductById(id) != null;
        }
    }
}

