using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ASPnetPhishing.Models
{
    public class Cart
    {
        public const decimal TAX = 0.07m;
        private List<LineItem> cartItems;

        public List<LineItem> CartItems 
        {
            get { return this.cartItems; }
            set { this.cartItems = value; }
        }

        public AspNetUser CartOwner { get; set; }
        public Invoice Invoice { get; set; }
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Total { get; private set; }
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Subtotal { get; private set; }
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal TaxAmount { get; private set; }

        public Cart()
        {
            cartItems = new List<LineItem>();
        }

        public void AddItem(LineItem li)
        {
            bool duplicate = false;
            foreach (LineItem item in this.cartItems)
            {
                if (li.Product.Id == item.Product.Id)
                {
                    duplicate = true;
                    item.Qty += 1;
                }
            }
            if (!duplicate)
            {
                this.CartItems.Add(li);
            }
            CalculateTotal();

        }

        public void UpdateItem(LineItem li)
        {
            foreach (LineItem item in this.cartItems)
            {
                if (li.Product.Id == item.Product.Id)
                {
                    item.Qty = li.Qty;
                }
            }
            CalculateTotal();
        }

        public void RemoveItem(LineItem li)
        {
            foreach (LineItem item in this.cartItems)
            {
                if (li.Product.Id == item.Product.Id)
                {
                    li = item;
                }
            }
            this.CartItems.Remove(li);
            CalculateTotal();
        }

        public void ClearCart()
        {
            this.CartItems.Clear();
            this.Total = 0m;
        }

        private void SetSubtotal()
        {
            this.Subtotal = 0m;
            foreach (LineItem li in this.CartItems)
            {
                this.Subtotal += li.LineTotal;
            }
        }

        private void SetTaxAmount()
        {
            this.TaxAmount = this.Subtotal * TAX;
        }

        private void CalculateTotal()
        {
            SetSubtotal();
            SetTaxAmount();
            this.Total = this.Subtotal + this.TaxAmount;
        }
    }
}