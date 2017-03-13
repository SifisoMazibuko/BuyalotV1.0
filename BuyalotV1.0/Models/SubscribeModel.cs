using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuyalotV1._0.Models
{
    public class SubscribeModel
    {

        [Key]
        public int subscribeID { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string email { get; set; }
    }
}