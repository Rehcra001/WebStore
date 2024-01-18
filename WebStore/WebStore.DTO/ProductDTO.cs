using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public decimal Price { get; set; }

        public int QtyInStock { get; set; }

        public int UnitPerId { get; set; }

        public string UnitPer { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
