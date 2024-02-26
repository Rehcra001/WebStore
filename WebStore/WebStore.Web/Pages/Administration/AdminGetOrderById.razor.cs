using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages.Administration
{
    public partial class AdminGetOrderById
    {
        [Inject]
        public IOrderService OrderService { get; set; }

        [Inject]
        public ICompanyService CompanyService { get; set; }

        private int OrderId { get; set; }

        private OrderDTO Order { get; set; } = new OrderDTO();

        private CompanyDetailDTO Company { get; set; } = new CompanyDetailDTO();

        private bool IsValidOrder { get; set; }

        private const decimal VAT = 0.15M;
        private const decimal VAT_PERCENTAGE = VAT * 100;

        protected override async Task OnInitializedAsync()
        {
            

            Company = await CompanyService.GetCompanyDetail();
            if (Company == null || Company.CompanyId == 0)
            {
                throw new Exception("Unable to retrieve company details");
            }
        }

        private async Task GetOrder_Click()
        {
            if (OrderId != 0)
            {
                Order = await OrderService.GetOrderById(OrderId);
                if (Order == null || Order.OrderId == 0)
                {
                    IsValidOrder = false;
                }
                else
                {
                    IsValidOrder = true;
                }
            }
        }

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
