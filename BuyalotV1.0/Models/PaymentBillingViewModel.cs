using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyalotV1._0.Models
{
    public class PaymentBillingViewModel
    {

        //public int paymentID { get; set; }
        public DateTime paymentDate { get; set; }
        public string paymentType { get; set; }
        public decimal totalPrice { get; set; }
        public string cardNumber { get; set; }
        public string cardType { get; set; }
        public DateTime expDate { get; set; }
        public string cardHolderName { get; set; }
        public string shippingAddress { get; set; }
        public int cvv { get; set; }
    }
}