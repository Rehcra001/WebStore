using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.DTO
{
    public class CompanyEFTDTO
    {
        public int EFTId { get; set; }
        public string Bank { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string BranchCode { get; set; } = string.Empty;
    }
}
