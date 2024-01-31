using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.DTO
{
    public class ProductCategoryDTO
    {
        public int ProductCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public byte[] Picture { get; set; }
    }
}
