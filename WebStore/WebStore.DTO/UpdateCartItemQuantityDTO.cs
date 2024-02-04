using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.DTO
{
    public class UpdateCartItemQuantityDTO
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
