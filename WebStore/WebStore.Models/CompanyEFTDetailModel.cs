using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class CompanyEFTDetailModel
    {
        public int EFTId { get; set; }
        [Required]
        public string Bank { get; set; } = string.Empty;
        [Required]
        public string AccountType { get; set; } = string.Empty;
        [Required]
        public string AccountNumber { get; set; } = string.Empty;
        [Required]
        public string BranchCode { get; set; } = string.Empty;
    }
}
