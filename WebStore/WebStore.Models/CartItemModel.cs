using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class CartItemModel
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }

        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
