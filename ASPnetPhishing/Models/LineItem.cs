//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASPnetPhishing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class LineItem
    {
        public int LineItemId { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }

        private int qty;
        public int? Qty
        {
            get
            {
                return this.qty;
            }
            set
            {
                this.qty = Convert.ToInt32(value);
                if (this.Product != null)
                {
                    SetLineTotal();
                }
            }
        }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal LineTotal { get; private set; }

        private void SetLineTotal()
        {
            this.LineTotal = this.qty * this.Product.Price;            
        }

        public LineItem()
        {

        }

        public LineItem(int invoiceId, int qty, Product product)
        {
            this.InvoiceId = invoiceId;
            this.qty = qty;
            this.Product = product;
        }
    
        public virtual Invoice Invoice { get; set; }
        private Product product;
        public virtual Product Product
        {
            get
            {
                return this.product;
            }
            set
            {
                this.product = value;
                this.ProductId = this.product.Id;
                if (this.Qty != null)
                {
                    SetLineTotal();
                }
            }
        }
    }
}
