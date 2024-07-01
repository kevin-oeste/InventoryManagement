using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models;

//trying to fix a lot of code in here using the github
//Doing this by changing the shopping cart from a list of products to the ShoppingCart model from the github

namespace InventoryManagement.Services
{

    public class ShoppingCartServiceProxy
    {
        private static ShoppingCartServiceProxy? instance;
        private static object? instanceLock = new object();
    private List<ShoppingCart> carts;
    public ReadOnlyCollection<ShoppingCart> Carts
    {
        get
        {
            return carts.AsReadOnly();
        }
    }

    public ShoppingCart Cart
    {
        get
        {
            if (!carts.Any())
            {
                var newCart = new ShoppingCart();
                carts.Add(newCart);
                return newCart;
            }
            return carts?.FirstOrDefault() ?? new ShoppingCart();
        }
    }
        public static ShoppingCartServiceProxy Current
        {

            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ShoppingCartServiceProxy();
                    }
                }
                return instance;
            }
        }
        public ReadOnlyCollection<Product>? cartItems
        {
            get
            {
                return Cart?.Contents?.AsReadOnly();
            }
        }
        private double taxRate = 0.07;
        public decimal Subtotal
        {
            get
            {
                decimal subtotal = 0;
                foreach (Product x in Cart?.Contents)
                {
                    subtotal += (decimal)(x.Amount * x.Price);
                }
                return subtotal;
            }
        }
        public decimal Taxes
        {
            get
            {
                return (decimal)taxRate * Subtotal;
            }
        }
        public decimal Total
        {
            get
            {
                return Subtotal + Taxes;
            }
        }

        public string Receipt
        {
            get
            {
                var receipt = "Thank you for shopping with us!\n";
                foreach (var item in Cart?.Contents)
                {
                    receipt += ($"{item.Amount} {item.Name}\t{item.Price}\n");
                }
                receipt += $"\n\nSubtotal: {Subtotal:C}\nTaxes: {Total:C}";
                return receipt;
            }
        }
        public string Checkout()
        {
            return Receipt;
        }

        private ShoppingCartServiceProxy()
        {
            carts = new List<ShoppingCart>();
        }
        public void AddProduct(Product newProduct, int addAmount)
        {
            if (Cart == null || Cart.Contents == null)
            {
                return;
            }
            var existingProduct = Cart?.Contents?
                .FirstOrDefault(existingProducts =>existingProducts.Id == newProduct.Id);
            var inventoryProduct = InventoryServiceProxy.Current.Items
                .FirstOrDefault(invProd => invProd.Id == newProduct.Id);
            
            inventoryProduct.Amount -= newProduct.Amount;

            if (inventoryProduct == null)
            {
                return;
            }
            if (existingProduct != null)
            {
                //update
                existingProduct.Amount += newProduct.Amount;
            }
            else
            {
                //add
                Cart?.Contents?.Add(newProduct);
            }
                
        }
        
        public void RemoveProduct(Product trash)
        {
            if(Cart == null || Cart.Contents == null) { return; }

            var existingProduct = Cart?.Contents?
      .FirstOrDefault(existingProducts => existingProducts.Id == trash.Id);
            var inventoryProduct = InventoryServiceProxy.Current.Items
                .FirstOrDefault(invProd => invProd.Id == trash.Id);
            //add the amount back to the inventory
            inventoryProduct.Amount += trash.Amount;
            //remove the product
            Cart?.Contents?.Remove(existingProduct);
        }

        private Product GetProductById(Guid id)
        {
            return Cart?.Contents?.FirstOrDefault(product => product.Id.Equals(id)) ?? new Product();
        }

        public bool ContainsItem(Guid id)
        {
            return GetProductById(id) != null;
        }

        /* not sure I need this anymore
        public override string ToString()
        {
            string output = String.Empty;
            if (Cart.Any())
            {
                foreach (Product product in Cart)
                {
                    //more workarounds -_-
                    output += ($"{product.Amount} {product.Name}\t{product.Price}\n");
                }
            }
            return output;
        }
        */
    }
}
