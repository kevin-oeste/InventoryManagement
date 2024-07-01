using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    //my shopping cart wasn't working, so may as well try this one
    public class ShoppingCart
    {
        int Id { get; set; }
        public List<Product>? Contents { get; set; }

        public ShoppingCart()
        {
            Contents = new List<Product>();
        }
    }
}
