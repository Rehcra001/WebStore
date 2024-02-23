namespace WebStore.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool PaymentConfirmed { get; set; }
        public bool OrderShipped { get; set; }
        public int AddressId { get; set; }
        public AddressModel Address { get; set; }
        public IEnumerable<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}
