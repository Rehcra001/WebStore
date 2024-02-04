using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages
{
    public partial class ProductDetail
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public ProductDTO Product { get; set; }

        private async Task AddToCartItem_Click(int productId, int quantity)
        {
            CartItemAddToDTO cartItemAddToDTO = new CartItemAddToDTO
            {
                ProductId = productId,
                Quantity = quantity
            };

            await ShoppingCartService.AddCartItem(cartItemAddToDTO);

            

            NavigationManager.NavigateTo("/shoppingcart");
        }
    }
}
