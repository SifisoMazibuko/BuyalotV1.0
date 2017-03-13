using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyalotV1._0.Models
{
    ///// <summary>
    ///// Class used to send information to the WebService, as well as store information from the WebService call
    ///// </summary>
    public class Transaction
    {
        public string Country { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public string Reference { get; set; }
        public string Status { get; set; }

        public string CountryName { get; set; }
        public string CurrencyName { get; set; }
        public string Date_Created { get; set; }
        public string Date_Ready { get; set; }
        public string Date_Completed { get; set; }
        public string TnxID { get; set; }
        public string ReceiptNo { get; set; }

        public Transaction(string _country, string _currency, string _amount, string _reference)
        {
            Country = _country;
            Currency = _currency;
            Amount = _amount;
            Reference = _reference;
        }

        public Transaction()
        {
        }
    }
}