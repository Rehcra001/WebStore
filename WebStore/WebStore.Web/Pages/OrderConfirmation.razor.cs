using Blazored.LocalStorage;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using WebStore.DTO;
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

        [Parameter]
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();

        private List<AddressLineDTO> AddressLines { get; set; } = new List<AddressLineDTO>();

        private int DefaultShipAddress { get; set; }

        private const decimal VAT = 0.15M;
        private const decimal VAT_PERCENTAGE = VAT * 100;

        protected override async Task OnInitializedAsync()
        {
            if (await LocalStorageService.ContainKeyAsync("CartItems"))
            {
                CartItems = await LocalStorageService.GetItemAsync<List<CartItemDTO>>("CartItems");

                //Retrieve addresses
                var returned = await CustomerService.GetAddresLinesAsync();

                if (returned != null || returned.Count() > 0)
                {
                    AddressLines = (List<AddressLineDTO>)returned;
                    //select the first address as ship to
                    DefaultShipAddress = AddressLines[0].AddressId;
                    AddressLineDTO addressLine = AddressLines.First(x => x.AddressId == DefaultShipAddress);
                    addressLine.ShipToSelected = true;
                }
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
