using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.DTO
{
    public class PaymentConfirmationDTO
    {
        public int OrderId { get; set; }
        public bool Payed { get; set; }
    }
}
