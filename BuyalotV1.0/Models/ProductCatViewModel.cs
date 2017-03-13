using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyalotV1._0.Models
{
    public class ProductCatViewModel
    {
        public int productID { get; set; }
        public string productName { get; set; }

        public string productDescription { get; set; }

        public string vendor { get; set; }

        public decimal price { get; set; }

        public int quantityInStock { get; set; }

        public byte[] productImage { get; set; }

        public string categoryName { get; set; }
        

    }
}