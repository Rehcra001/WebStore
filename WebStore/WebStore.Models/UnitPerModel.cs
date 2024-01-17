using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class UnitPerModel
    {
        public int UnitPerId { get; set; }

        [Required(ErrorMessage = "Unit per is required.")]
        [StringLength(10, ErrorMessage = "Must be between {2} and {1} characters.", MinimumLength = 1)]
        [Display(Name = "Unit Per")]
        public string UnitPer { get; set; }
    }
}
