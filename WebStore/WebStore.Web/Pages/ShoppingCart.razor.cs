using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages
{
    public partial class ShoppingCart
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        private List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();

        private decimal TotalPrice { get; set; }

        private int TotalQuantity { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (await LocalStorageService.ContainKeyAsync("CartItems"))
                {
                    await LocalStorageService.RemoveItemAsync("CartItems");
                }
                var items = await ShoppingCartService.GetCartItems();

                if (items.Count() > 0)
                {
                    CartItems = items.ToList();
                }
                CartChanged();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }    
        
        private async Task DeleteCartItem(int id)
        {
            await ShoppingCartService.DeleteCartItem(id);

            await GetCartItemsAsync();
        }

        private async void UpdateItemQuantity(int id)
        {
            CartItemDTO cartItem = CartItems.First(x => x.CartItemId == id);

            if (cartItem.Quantity <= 0)
            {
                cartItem.Quantity = 1;
            }

            UpdateCartItemQuantityDTO updateCartItem = new UpdateCartItemQuantityDTO
            {
                CartItemId = cartItem.CartItemId,                
                Quantity = cartItem.Quantity
            };

            await ShoppingCartService.UpdateCartItemQuantity(updateCartItem);

            await GetCartItemsAsync();
            
        }

        private async Task GetCartItemsAsync()
        {
            CartItems.Clear();

            var items = await ShoppingCartService.GetCartItems();

            if (items.Count() > 0)
            {
                CartItems = items.ToList();
            }
            CartChanged();
            StateHasChanged();
        }

        private void CartChanged()
        {
            TotalPrice = CartItems.Sum(x => x.Quantity * x.Price);
            TotalQuantity = CartItems.Sum(x => x.Quantity);

            ShoppingCartService.RaiseShoppingCartChangedEvent(TotalQuantity);
        }

        private async Task PreviewOrder_Click()
        {
            if (CartItems.Count > 0)
            {
                //clear local storage of cart items if it exists
                if (await LocalStorageService.ContainKeyAsync("CartItems"))
                {
                    await LocalStorageService.RemoveItemAsync("CartItems");
                }
                    await LocalStorageService.SetItemAsync("CartItems", CartItems);
                NavigationManager.NavigateTo($"/orderpreview");
            }
            
        }
    }
}
