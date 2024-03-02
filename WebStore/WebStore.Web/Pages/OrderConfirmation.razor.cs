using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages
{
    public partial class OrderConfirmation
    {
        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorage { get; set; }

        [Inject]
        public ICustomerService CustomerService { get; set; }

        [Inject]
        public ICompanyService CompanyService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Parameter]
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();

        private CustomerDTO Customer { get; set; } = new CustomerDTO();

        private AddressDTO ShippingAddress { get; set; } = new AddressDTO();

        private CompanyDetailDTO Company { get; set; } = new CompanyDetailDTO();

        private bool OrderConfirmed { get; set; }

        private const decimal VAT = 0.15M;
        private const decimal VAT_PERCENTAGE = VAT * 100;

        protected override async Task OnInitializedAsync()
        {
            //Get cart items
            
            CartItems = (List<CartItemDTO>)await ManageCartItemsLocalStorage.GetCollection();
            
            if (CartItems.Count == 0)
            {
                throw new Exception("No Cart Items Found");
            }

            //Get EFT Detail
            Company = await CompanyService.GetCompanyDetail();

            //Get Customer
            Customer = await CustomerService.GetCustomerDetailsAsync();

            //Get shipping address
            if (await LocalStorageService.ContainKeyAsync("ShippingAddress"))
            {
                int addressId = await LocalStorageService.GetItemAsync<int>("ShippingAddress");

                if (addressId != default)
                {
                    //retrieve address
                    ShippingAddress = Customer.AddressList.FirstOrDefault(x => x.AddressId == addressId);

                    if (ShippingAddress == null || ShippingAddress.AddressId == default)
                    {
                        throw new Exception("Address not found");
                    }
                }
            }
            else
            {
                throw new Exception("Error retrieving data");
            }
        }

        private async Task ConfirmOrder_Click()
        {
            OrderDTO order = await CustomerService.AddOrderAsync(ShippingAddress.AddressId);
            if (order != null && order.OrderId > 0)
            {
                OrderConfirmed = true;
                //clear out local storage
                if (await LocalStorageService.ContainKeyAsync("ShippingAddress"))
                {
                    await LocalStorageService.RemoveItemAsync("ShippingAddress");
                }
                await ManageCartItemsLocalStorage.RemoveCollection();

                // clear count of items in shopping cart menu 
                ShoppingCartService.RaiseShoppingCartChangedEvent(0);
            }
        }

        private async void GoToHomePage()
        {
            NavigationManager.NavigateTo("/");
        }

        private decimal CalcLinePrice(int quantity, decimal price)
        {
            return price * quantity;
        }

        private decimal CalcTotalPrice()
        {
            return CartItems.Sum(x => x.Quantity * x.Price);
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
