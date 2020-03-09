using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eslog2_0.Models
{
    public class EslogData
    {
        public Invoice invoice { get; set; }
        public Sender sender { get; set; }
        public Receiver receiver { get; set; }
        public List<ReferenceDocument> referenceDocuments { get; set; }
        public List<InvoiceItem> invoiceItems { get; set; }

    }
}
