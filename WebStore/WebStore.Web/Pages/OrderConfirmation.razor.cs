using Blazored.LocalStorage;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services;
using WebStore.WEB.Services.Contracts;
using WebStore.WEB.Validators;

namespace WebStore.WEB.Pages
{
    public partial class OrderConfirmation
    {
        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        [Inject]
        public ICustomerService CustomerService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Parameter]
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();

        private AddressDTO ShippingAddress { get; set; } = new AddressDTO();

        private const decimal VAT = 0.15M;
        private const decimal VAT_PERCENTAGE = VAT * 100;

        protected override async Task OnInitializedAsync()
        {
            //Get cart items
            if (await LocalStorageService.ContainKeyAsync("CartItems"))
            {
                CartItems = await LocalStorageService.GetItemAsync<List<CartItemDTO>>("CartItems");                
            }
            else
            {
                CartItems = (List<CartItemDTO>)await ShoppingCartService.GetCartItems();
            }
            if (CartItems.Count == 0)
            {
                throw new Exception("No Cart Items Found");
            }

            //Get shipping address
            if (await LocalStorageService.ContainKeyAsync("ShippingAddress"))
            {
                int addressId = await LocalStorageService.GetItemAsync<int>("ShippingAddress");

                if (addressId != default)
                {
                    //retrieve address
                    ShippingAddress = await CustomerService.GetAddressByIdAsync(addressId);
                    if (ShippingAddress.AddressId == default)
                    {
                        throw new Exception("Address not found");
                    }
                }
            }
            else
            {
                throw new Exception("No Shipping Address ID Found");
            }
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
