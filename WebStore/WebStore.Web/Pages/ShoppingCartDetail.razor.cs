using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages
{
    public partial class ShoppingCartDetail : ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Parameter]
        public CartItemDTO CartItem { get; set; }

        private const string HIDE_UPDATE = "hide-update";
        private const string SHOW_UPDATE = "show-update";

        private string UpdateQtyVisibility = HIDE_UPDATE;

        private void ShowUpdateButton()
        {
            UpdateQtyVisibility = SHOW_UPDATE;
        }

        private async Task UpdateCartItemQty()
        {
            UpdateQtyVisibility = HIDE_UPDATE;
        }

    }
}
