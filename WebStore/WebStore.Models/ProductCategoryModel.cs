using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class ProductCategoryModel
    {
        public int ProductCategoryId { get; set; }

        [Required(ErrorMessage = "Product Category Name is required.")]
        [StringLength(100, ErrorMessage = "Must be between {2} and {1} characters.", MinimumLength = 3)]
        [Display(Name = "Product Category Name")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Picture is required")]
        [Display(Name = "Category Picture")]
        public byte[] Picture { get; set; }
    }
}
