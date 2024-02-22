using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages
{
    public partial class ProductDetail
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorage { get; set; }

        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public ProductDTO Product { get; set; }

        private async Task AddToCartItem_Click(int productId, int quantity)
        {
            var products = await ManageProductsLocalStorageService.GetCollection();
            ProductDTO productDTO = products.First(x => x.ProductId == productId);
            
            if (productDTO.QtyInStock > 0)
            {
                CartItemAddToDTO cartItemAddToDTO = new CartItemAddToDTO
                {
                    ProductId = productId,
                    Quantity = quantity
                };

                List<CartItemDTO> cartItems = (List<CartItemDTO>)await ManageCartItemsLocalStorage.GetCollection();
                var cartItem = await ShoppingCartService.AddCartItem(cartItemAddToDTO);
                cartItems.Add(cartItem);
                await ManageCartItemsLocalStorage.SaveCollection(cartItems);

                NavigationManager.NavigateTo("/shoppingcart");
            }            
        }
    }
}
