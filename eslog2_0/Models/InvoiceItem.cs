using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eslog2_0.Models
{
    public class InvoiceItem
    {
        public string itemText { get; set; } //item name or ID or anything basically that indicates the item
        public int quantity { get; set; }
        public decimal price { get; set; }
        public decimal totalPrice { get; set; } //quantity * price + VAT
        public decimal vatPercent { get; set; } //% of VAT
        public decimal vatAmount { get; set; } // amount of VAT
        public decimal discountPercent { get; set; }
        public decimal discountAmount { get; set; } 
        public string vatCatergory { get; set; } //category code from UNTDID 5305; if extempted from vat, coded statement is mandatory in eslog (reference)
        public string vatType { get; set; } //Is it a standard VAT or EXT?
    }
}
