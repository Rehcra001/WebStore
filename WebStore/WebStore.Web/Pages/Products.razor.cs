using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages
{
    public partial class Products
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Category { get; set; } = string.Empty;

        private List<ProductDTO> ProductList { get; set; } = new List<ProductDTO>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var products = await ProductService.GetProductsAsync();

                if (products.Count() > 0)
                {
                    ProductList = products.Where(x => x.CategoryName.Equals(Category)).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

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
