using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.DTO
{
    public class CompanyDetailDTO
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public byte[] CompanyLogo { get; set; }
        public int AddressId { get; set; }
        public int EFTId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;

        public AddressDTO CompanyAddress { get; set; } = new AddressDTO();
        public CompanyEFTDTO CompanyEFT { get; set; } = new CompanyEFTDTO();
    }
}
