using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Must be between {2} and {1} characters.",MinimumLength = 3)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product description is required.")]
        [StringLength(250, ErrorMessage = "Must be between {2} and {1} characters.", MinimumLength = 5)]
        [Display(Name = "Product Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Product picture is required.")]
        [Display(Name = "Product Picture")]
        public byte[] Picture { get; set; }
                
        [Display(Name = "Product price")]
        public decimal Price { get; set; }
        
        [Display(Name = "Product Picture")]
        public int QtyInStock { get; set; }

        [Required(ErrorMessage = "Product Unit Per Id is required.")]
        [Display(Name = "Product Unit Per Id")]
        public int UnitPerId { get; set; }

        public string UnitPer { get; set; }

        [Required(ErrorMessage = "Product Category Id is required.")]
        [Display(Name = "Product Unit Per Id")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
