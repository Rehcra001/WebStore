using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.DTO
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
