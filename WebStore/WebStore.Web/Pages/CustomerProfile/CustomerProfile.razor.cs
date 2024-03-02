using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages.CustomerProfile
{
    public partial class CustomerProfile
    {
        [Inject]
        public IManageCustomerDetailLocalStorage ManageCustomerDetailLocalStorage { get; set; }

        [Inject]
        public IManageCustomerOrdersLocalStorage ManageCustomerOrdersLocalStorage { get; set; }

        [Inject]
        public ICompanyService CompanyService { get; set; }

        private string ErrorMessage { get; set; } = string.Empty;
        private string? PageToView { get; set; }
        private string[] CustomerDetail = new string[] { "CustomerDetail" };
        private string[] CustomerOrders = new string[] { "PaymentOutstanding", "ShippingOutstanding", "Completed" };

        public CustomerDTO Customer { get; set; }
        private IEnumerable<OrderDTO> Orders { get; set; } = new List<OrderDTO>();
        private List<OrderDTO> OrdersPayment { get; set; } = new List<OrderDTO>();
        private List<OrderDTO> OrdersShipping { get; set; } = new List<OrderDTO>();
        private List<OrderDTO> OrdersComplete { get; set; } = new List<OrderDTO>();
        //private int OrderId { get; set; }
        private OrderDTO Order { get; set; } = new OrderDTO();
        private CompanyDetailDTO CompanyDetail { get; set; } = new CompanyDetailDTO();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                //Clear local storage
                await ManageCustomerDetailLocalStorage.RemoveCustomerDetail();
                await ManageCustomerOrdersLocalStorage.RemoveCollection();

                //Retrieve all customer info
                Customer = await ManageCustomerDetailLocalStorage.GetCustomerDetail();
                Orders = await ManageCustomerOrdersLocalStorage.GetCollection();
                CompanyDetail = await CompanyService.GetCompanyDetail();

                OrderWaitingPayment();
                OrdersWaitingShipping();
                OrdersCompleted();

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void OpenPaymentOrder_Click(int id)
        {
            Order = Orders.First(x => x.OrderId == id);
            PageToView = CustomerOrders[0];
        }

        private void OpenShipOrder_Click(int id)
        {
            Order = Orders.First(x => x.OrderId == id);
            PageToView = CustomerOrders[1];
        }

        private void OpenCompleteOrder_Click(int id)
        {
            Order = Orders.First(x => x.OrderId == id);
            PageToView = CustomerOrders[2];
        }

        private void OrdersCompleted()
        {
            OrdersComplete = Orders.Where(x => x.PaymentConfirmed == true && x.OrderShipped == true).ToList();
        }

        private void OrdersWaitingShipping()
        {
            OrdersShipping = Orders.Where(x => x.PaymentConfirmed == true && x.OrderShipped == false).ToList();
        }

        private void OrderWaitingPayment()
        {
            OrdersPayment = Orders.Where(x => x.PaymentConfirmed == false).ToList();
        }

        private void Menu_Click(string pageToView)
        {
            PageToView = pageToView;
        }

    }
}
