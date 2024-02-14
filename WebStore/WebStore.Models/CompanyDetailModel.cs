using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class CompanyDetailModel
    {
        public int CompanyId { get; set; }

        [Required]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        public byte[] CompanyLogo { get; set; }

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string EmailAddress { get; set; } = string.Empty;

        public int AddressId { get; set; }

        public int EFTId { get; set; }
        
    }
}
