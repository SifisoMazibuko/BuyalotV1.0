using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyalotV1._0.Models
{
    public class InvoiceViewModel
    {
        public int InvoiceID { get; set; }
        public int OrderID { get; set; }
        public String Items { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}