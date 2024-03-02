using Microsoft.AspNetCore.Components;
using WebStore.DTO;

namespace WebStore.WEB.Pages.CustomerProfile
{
    public partial class CustomerOrder
    {
        [Parameter]
        public OrderDTO Order { get; set; } = new OrderDTO();

        [Parameter]
        public CompanyDetailDTO Company { get; set; } = new CompanyDetailDTO();

        private const decimal VAT = 0.15M;
        private const decimal VAT_PERCENTAGE = VAT * 100;

        private decimal CalcLinePrice(int quantity, decimal price)
        {
            return price * quantity;
        }

        private decimal CalcTotalPrice()
        {
            return Order.OrderItems.Sum(x => x.Quantity * x.Price);
        }

        private decimal CalcVAT()
        {
            return CalcTotalPrice() * VAT;
        }

        private decimal CalcTotalWithVAT()
        {
            return CalcTotalPrice() + CalcVAT();
        }
    }
}
