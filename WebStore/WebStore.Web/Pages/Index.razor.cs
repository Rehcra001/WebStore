using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Security.Claims;
using WebStore.DTO;
using WebStore.WEB.Providers;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages
{
    public partial class Index
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public AppAuthenticationStateProvider AuthenticatedUser { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();

        private List<ProductCategoryDTO> ProductCategories { get; set; } = new List<ProductCategoryDTO>();

        private string WebstoreTitle { get; set; } = "Web Store";

        protected override async Task OnInitializedAsync()
        {
            var productCategories = await ProductService.GetProductCategoriesAsync();

            if (productCategories.Count() > 0)
            {
                ProductCategories = (List<ProductCategoryDTO>)productCategories;
            }

            var authState = await AuthenticatedUser.GetAuthenticationStateAsync();

            var user = authState.User;

            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                //Get cartitems if any and update shopping cart menu
                // TODO Look at using local storage for adding products and cart items
                var items = await ShoppingCartService.GetCartItems();
                var quantity = items.Sum(x => x.Quantity);
                ShoppingCartService.RaiseShoppingCartChangedEvent(quantity);
            }
        }
    }
}

