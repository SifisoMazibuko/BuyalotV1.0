using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuyalotV1._0.Models
{
    public class CustomerAddressModel
    {
        [Required(ErrorMessage = "First Name required")]
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Required(ErrorMessage = "Last Name required")]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string phone { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]       
        public string confirmPassword { get; set; }
        public string state { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
    }
}