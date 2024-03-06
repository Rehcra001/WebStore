using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Security.Claims;
using WebStore.DTO;
using WebStore.WEB.Providers;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages
{
    public partial class Index
    {
        [Inject]
        public AppAuthenticationStateProvider AuthenticatedUser { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        [Inject]
        public IManageProductCategoriesLocalStorageService ManageProductCategoriesLocalStorageService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        [Inject]
        public IManageAdminPaymentOrdersLocalStorageService ManageAdminPaymentOrdersLocalStorageService { get; set; }

        [Inject]
        public IManageAdminShippedOrdersLocalStorageService ManageAdminShippedOrdersLocalStorageService { get; set; }

        [Inject]
        public IManageCustomerDetailLocalStorage ManageCustomerDetailLocalStorage { get; set; }

        [Inject]
        public IManageCustomerOrdersLocalStorage ManageCustomerOrdersLocalStorage { get; set; }

        private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();

        private List<ProductCategoryDTO> ProductCategories { get; set; } = new List<ProductCategoryDTO>();

        private string WebstoreTitle { get; set; } = "Web Store";

        protected override async Task OnInitializedAsync()
        {
            await ClearLocalStorage();

            // Add product categories to local storage
            var productCategories = await ManageProductCategoriesLocalStorageService.GetCollection();

            if (productCategories.Count() > 0)
            {
                ProductCategories = (List<ProductCategoryDTO>)productCategories;
            }

            // Add Products to local storage
            await ManageProductsLocalStorageService.GetCollection();

            var authState = await AuthenticatedUser.GetAuthenticationStateAsync();

            var user = authState.User;

            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                //Get cartitems if any and update shopping cart menu
                // TODO Look at using local storage for adding products and cart items
                var items = await ManageCartItemsLocalStorageService.GetCollection();
                var quantity = items.Sum(x => x.Quantity);
                ShoppingCartService.RaiseShoppingCartChangedEvent(quantity);
            }
        }

        private async Task ClearLocalStorage()
        {
            await ManageProductsLocalStorageService.RemoveCollection();
            await ManageProductCategoriesLocalStorageService.RemoveCollection();
            await ManageCartItemsLocalStorageService.RemoveCollection();
            await ManageAdminPaymentOrdersLocalStorageService.RemoveCollection();
            await ManageAdminShippedOrdersLocalStorageService.RemoveCollection();
            await ManageCustomerDetailLocalStorage.RemoveCustomerDetail();
            await ManageCustomerOrdersLocalStorage.RemoveCollection();
        }

        
    }
}

