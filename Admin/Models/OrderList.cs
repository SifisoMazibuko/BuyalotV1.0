using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class OrderList
    {
        public int customerID { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime shippingDate { get; set; }
        public string shippingAddress { get; set; }
        public string status { get; set; }
        public int quantityOrdered { get; set; }
        public decimal priceEach { get; set; }
        public string productName { get; set; }

       
        
    }
}