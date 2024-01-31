using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.DTO
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }
        public string EmailAddress { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public IEnumerable<AddressDTO> AddressList { get; set; }
    }
}
