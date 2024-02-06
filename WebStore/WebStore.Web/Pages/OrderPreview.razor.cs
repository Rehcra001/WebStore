using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using WebStore.DTO;

namespace WebStore.WEB.Pages
{
    public partial class OrderPreview
    {
        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        [Parameter]
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();

        private List<AddressLineDTO> AddressLines { get; set; } = new List<AddressLineDTO>();

        private const decimal VAT = 0.15M;
        private const decimal VAT_PERCENTAGE = VAT * 100;


        protected override async Task OnInitializedAsync()
        {
            if (await LocalStorageService.ContainKeyAsync("CartItems"))
            {
                CartItems = await LocalStorageService.GetItemAsync<List<CartItemDTO>>("CartItems");
                //clear local storage of cart items
                await LocalStorageService.RemoveItemAsync("CartItems");

                //Retrieve addresses

                //select the first address as ship to


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
