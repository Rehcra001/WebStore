using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool PaymentConfirmed { get; set; }
        public bool OrderShipped { get; set; }
        public int AddressId { get; set; }
        public AddressDTO Address { get; set; } = new AddressDTO();
        public IEnumerable<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}
