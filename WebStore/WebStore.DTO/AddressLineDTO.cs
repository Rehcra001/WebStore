using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.DTO
{
    public class AddressLineDTO
    {
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; } = string.Empty;
        public bool ShipToSelected { get; set; }
    }
}
