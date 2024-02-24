using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages.Administration
{
    public partial class AdminOrderConfirmation
    {
        [Inject]
        public IManageAdminShippedOrdersLocalStorageService ManageAdminShippedOrdersLocalStorageService { get; set; }

        [Inject]
        public IManageAdminPaymentOrdersLocalStorageService ManageAdminPaymentOrdersLocalStorageService { get; set; }


        [Parameter, EditorRequired]
        public string ActionType { get; set; } = string.Empty;

        private int _orderId = -1;
        private int OrderId
        {
            get => _orderId;
            set
            {
                _orderId = value;
                OnSelectionChanged(value);
            }
        }

        private List<OrderDTO> Orders { get; set; } = new List<OrderDTO>();
        private OrderDTO Order { get; set; } = new OrderDTO();

        private string Heading { get; set; } = string.Empty;

        private const string PAYMENT_TYPE = "Payment";
        private const string SHIPMENT_TYPE = "Shipment";
        private const decimal VAT = 0.15M;
        private const decimal VAT_PERCENTAGE = VAT * 100;

        protected override async Task OnInitializedAsync()
        {
            if (ActionType.Equals(PAYMENT_TYPE))
            {
                Orders = (List<OrderDTO>)await ManageAdminPaymentOrdersLocalStorageService.GetCollection();
                Heading = "Payment Confirmation";
            }
            else if (ActionType.Equals(SHIPMENT_TYPE))
            {
                Orders = (List<OrderDTO>)await ManageAdminShippedOrdersLocalStorageService.GetCollection();
                Heading = "Shipping Confirmation";
            }
        }

        private void OnSelectionChanged(int value)
        {
            if (value == -1)
            {
                Order = new OrderDTO();
            }
            else
            {
                Order = Orders.First(x => x.OrderId == value);
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
