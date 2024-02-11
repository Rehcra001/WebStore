﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool OrderConfirmed { get; set; }
        public bool OrderShipped { get; set; }
        public int AddressId { get; set; }
        public IEnumerable<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}