using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class AddressModel
    {
        public int AddressId { get; set; }

        [Required(ErrorMessage = "Address Line 1 is required")]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Compare(nameof(AddressLine1), ErrorMessage = "Cannot be the same as Address Line 1")]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Suburb is required")]
        [Display(Name = "Suburb")]
        public string Suburb { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Customer Id is required")]
        [Display(Name = "Customer Id")]
        public int CustomerId { get; set; }

    }
}
