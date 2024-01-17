﻿using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100, ErrorMessage = "First name cannot have more than 100 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot have more than 100 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "Email address cannot have more than 100 characters")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(20, ErrorMessage = "Phone number cannot have more than 20 characters")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public IEnumerable<AddressModel> AddressList { get; set; }
    }
}
