using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages.Administration
{
    public partial class AdminOrderConfirmation
    {
        private const string PAYMENT_TYPE = "Payment";
        private const string SHIPMENT_TYPE = "Shipment";
        private const decimal VAT = 0.15M;
        private const decimal VAT_PERCENTAGE = VAT * 100;
        private const int NO_SELECTION = -1;

        [Inject]
        public IManageAdminShippedOrdersLocalStorageService ManageAdminShippedOrdersLocalStorageService { get; set; }

        [Inject]
        public IManageAdminPaymentOrdersLocalStorageService ManageAdminPaymentOrdersLocalStorageService { get; set; }

        [Inject]
        public IOrderService OrderService { get; set; }

        [Parameter, EditorRequired]
        public string ActionType { get; set; } = string.Empty;

        private int _orderId = NO_SELECTION;
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
            if (value == NO_SELECTION)
            {
                Order = new OrderDTO();
            }
            else
            {
                Order = Orders.First(x => x.OrderId == value);
            }
        }

        private async Task ConfirmPayment_Click()
        {
            await OrderService.UpdateOrderPayment(Order.OrderId, true);

            Order.PaymentConfirmed = true;

            List<OrderDTO> paymentOrders = (List<OrderDTO>)await ManageAdminPaymentOrdersLocalStorageService.GetCollection();

            //Add shipping confirmation
            List<OrderDTO> shippingOrders = (List<OrderDTO>)await ManageAdminShippedOrdersLocalStorageService.GetCollection();
            shippingOrders.Add(Order);

            //remove from local storage
            paymentOrders.RemoveAt(Orders.FindIndex(x => x.OrderId == Order.OrderId));
            await ManageAdminPaymentOrdersLocalStorageService.SaveCollection(paymentOrders);

            //Reload from Local storage
            Orders = (List<OrderDTO>)await ManageAdminPaymentOrdersLocalStorageService.GetCollection();
            await ManageAdminShippedOrdersLocalStorageService.SaveCollection(shippingOrders);

            OrderId = NO_SELECTION;
        }

        private async Task ConfirmShipping_Click()
        {
            await OrderService.UpdateOrderShipped(Order.OrderId, true);

            Order.OrderShipped = true;

            //Remove order from Local storage
            List<OrderDTO> shippingOrders = (List<OrderDTO>)await ManageAdminShippedOrdersLocalStorageService.GetCollection();
            shippingOrders.RemoveAt(Orders.FindIndex(x => x.OrderId == Order.OrderId));
            await ManageAdminShippedOrdersLocalStorageService.SaveCollection(shippingOrders);

            //Reload orders
            Orders = (List<OrderDTO>)await ManageAdminShippedOrdersLocalStorageService.GetCollection();

            OrderId = NO_SELECTION;
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
