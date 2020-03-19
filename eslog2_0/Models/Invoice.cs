using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eslog2_0.Models
{
    public class Invoice
    {
        public string invoiceType { get; set; }
        public string invoiceNumber { get; set; }
        public string paymentTerms { get; set; }
        public string paymentReference { get; set; }
        public DateTime invoiceDate { get; set; }
        public DateTime dueDate { get; set; }
        public DateTime serviceDate { get; set; }
        public string currency { get; set; }
        public decimal? totalAmount { get; set; }
        public decimal? vatPercentage { get; set; }
        public decimal? vatAmount { get; set; }
        public decimal? discountAmount { get; set; }
        public decimal paymentAmount { get; set; }

    }
}
